using System.Diagnostics;
using Converte_ICO.Models;
using Converte_ICO.Services;
using Microsoft.AspNetCore.Mvc;

namespace Converte_ICO.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImageConverterService _imageConverterService;

        public HomeController(ILogger<HomeController> logger, IImageConverterService imageConverterService)
        {
            _logger = logger;
            _imageConverterService = imageConverterService;
        }

        public IActionResult Index()
        {
            var model = new ImageUploadViewModel();
            
            // Recover image from TempData if available
            if (TempData["ImagePreviewUrl"] != null)
            {
                model.ImagePreviewUrl = TempData["ImagePreviewUrl"].ToString();
                // Keep preview url and file info so the next request can still use them
                TempData.Keep("ImagePreviewUrl");
                TempData.Keep("UploadedTempFileName");
                TempData.Keep("UploadedOriginalFileName");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(ImageUploadViewModel model)
        {
            try
            {
                if (model.ImageFile == null || model.ImageFile.Length == 0)
                {
                    ModelState.AddModelError("ImageFile", "Por favor, selecione uma imagem.");
                    return View("Index", model);
                }

                if (!_imageConverterService.IsValidImageFile(model.ImageFile))
                {
                    ModelState.AddModelError("ImageFile", "Formato de arquivo inválido. Aceitos: PNG, JPG, JPEG, SVG (máximo 10MB).");
                    return View("Index", model);
                }

                // Save uploaded file to temp folder (will be used for preview and conversion)
                var uploadedExt = Path.GetExtension(model.ImageFile.FileName);
                var tempFileName = $"upload_{Guid.NewGuid()}{uploadedExt}";
                var tempDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp");
                if (!Directory.Exists(tempDir))
                    Directory.CreateDirectory(tempDir);

                var savedPath = Path.Combine(tempDir, tempFileName);
                using (var stream = new FileStream(savedPath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                // Store only small data in TempData
                TempData["UploadedTempFileName"] = tempFileName; // file name only
                TempData["UploadedOriginalFileName"] = model.ImageFile.FileName;

                // Build preview URL
                model.ImagePreviewUrl = $"/temp/{tempFileName}";
                TempData["ImagePreviewUrl"] = model.ImagePreviewUrl;

                model.IsConverted = false;

                // Keep keys for subsequent requests
                TempData.Keep("ImagePreviewUrl");
                TempData.Keep("UploadedTempFileName");
                TempData.Keep("UploadedOriginalFileName");

                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no upload da imagem");
                ModelState.AddModelError("", "Erro interno do servidor. Tente novamente.");
                return View("Index", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Convert(ImageUploadViewModel model)
        {
            try
            {
                // Recover saved file name and original name
                var tempFileName = TempData["UploadedTempFileName"] as string;
                var originalFileName = TempData["UploadedOriginalFileName"] as string;
                var previewUrl = TempData["ImagePreviewUrl"] as string;

                // If we have a temp file from previous upload, remove validation error for ImageFile
                if (!string.IsNullOrEmpty(tempFileName))
                {
                    ModelState.Remove("ImageFile");
                }

                if (string.IsNullOrEmpty(tempFileName) || string.IsNullOrEmpty(originalFileName))
                {
                    ModelState.AddModelError("", "Sessão expirada. Por favor, faça o upload da imagem novamente.");
                    return RedirectToAction("Index");
                }

                var tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp", tempFileName);
                if (!System.IO.File.Exists(tempFilePath))
                {
                    ModelState.AddModelError("", "Arquivo temporário não encontrado. Faça o upload novamente.");
                    return RedirectToAction("Index");
                }

                // Create IFormFile from saved temp file
                await using var fs = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var formFile = new FormFile(fs, 0, fs.Length, "ImageFile", originalFileName);

                // Collect selected sizes
                var selectedSizes = new List<int>();
                if (model.Size16) selectedSizes.Add(16);
                if (model.Size32) selectedSizes.Add(32);
                if (model.Size64) selectedSizes.Add(64);
                if (model.Size128) selectedSizes.Add(128);

                if (!selectedSizes.Any())
                {
                    ModelState.AddModelError("", "Selecione pelo menos um tamanho para conversão.");
                    // Keep TempData and show the view again
                    TempData.Keep("ImagePreviewUrl");
                    TempData.Keep("UploadedTempFileName");
                    TempData.Keep("UploadedOriginalFileName");
                    model.ImagePreviewUrl = previewUrl;
                    return View("Index", model);
                }

                if (string.IsNullOrWhiteSpace(model.CustomFileName))
                {
                    model.CustomFileName = "icon";
                }

                var convertedFileName = await _imageConverterService.ConvertToIcoAsync(
                    formFile,
                    selectedSizes,
                    model.CustomFileName
                );

                model.ConvertedImagePath = convertedFileName;
                model.IsConverted = true;
                model.SelectedSizes = selectedSizes;
                model.ImagePreviewUrl = previewUrl;

                TempData["SuccessMessage"] = "Imagem convertida com sucesso!";
                // Keep for potential subsequent actions
                TempData.Keep("ImagePreviewUrl");
                TempData.Keep("UploadedTempFileName");
                TempData.Keep("UploadedOriginalFileName");
                
                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na conversão da imagem");
                ModelState.AddModelError("", $"Erro na conversão: {ex.Message}");
                TempData.Keep("ImagePreviewUrl");
                TempData.Keep("UploadedTempFileName");
                TempData.Keep("UploadedOriginalFileName");
                model.ImagePreviewUrl = TempData["ImagePreviewUrl"]?.ToString();
                return View("Index", model);
            }
        }

        [HttpGet]
        public IActionResult Download(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return NotFound();

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp", fileName);
                if (!System.IO.File.Exists(filePath))
                    return NotFound();

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var downloadFileName = fileName.Contains("_") ?
                    fileName.Substring(0, fileName.IndexOf("_")) + ".ico" :
                    fileName;

                return File(fileBytes, "image/x-icon", downloadFileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no download do arquivo");
                return NotFound();
            }
        }

        // New action to clear temp data and return to initial upload form
        [HttpGet]
        public IActionResult Reset()
        {
            try
            {
                var tempFileName = TempData["UploadedTempFileName"] as string;
                if (!string.IsNullOrEmpty(tempFileName))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp", tempFileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falha ao tentar deletar arquivo temporário durante reset.");
            }

            TempData.Remove("ImagePreviewUrl");
            TempData.Remove("UploadedTempFileName");
            TempData.Remove("UploadedOriginalFileName");
            TempData.Remove("SuccessMessage");

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
