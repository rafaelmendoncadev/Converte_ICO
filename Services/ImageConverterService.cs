using ImageMagick;

namespace Converte_ICO.Services
{
    public interface IImageConverterService
    {
        Task<string> ConvertToIcoAsync(IFormFile imageFile, List<int> sizes, string fileName);
        bool IsValidImageFile(IFormFile file);
    }

    public class ImageConverterService : IImageConverterService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string[] _allowedExtensions = { ".png", ".jpg", ".jpeg", ".svg" };
        private readonly long _maxFileSize = 10 * 1024 * 1024; // 10MB

        public ImageConverterService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public bool IsValidImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            if (file.Length > _maxFileSize)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return _allowedExtensions.Contains(extension);
        }

        public async Task<string> ConvertToIcoAsync(IFormFile imageFile, List<int> sizes, string fileName)
        {
            if (!IsValidImageFile(imageFile))
                throw new ArgumentException("Arquivo inválido");

            if (!sizes.Any())
                sizes = new List<int> { 16, 32, 64, 128 }; // Tamanhos padrão

            // Criar diretório temporário se não existir
            var tempPath = Path.Combine(_webHostEnvironment.WebRootPath, "temp");
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            // Nome único para o arquivo
            var uniqueFileName = $"{fileName}_{Guid.NewGuid()}.ico";
            var outputPath = Path.Combine(tempPath, uniqueFileName);

            using (var inputStream = imageFile.OpenReadStream())
            {
                // Para SVG, precisamos converter primeiro para PNG
                if (Path.GetExtension(imageFile.FileName).ToLowerInvariant() == ".svg")
                {
                    await ConvertSvgToIcoAsync(inputStream, outputPath, sizes);
                }
                else
                {
                    await ConvertImageToIcoAsync(inputStream, outputPath, sizes);
                }
            }

            return uniqueFileName;
        }

        private async Task ConvertImageToIcoAsync(Stream inputStream, string outputPath, List<int> sizes)
        {
            using (var collection = new MagickImageCollection())
            {
                using (var image = new MagickImage(inputStream))
                {
                    foreach (var size in sizes.OrderBy(s => s))
                    {
                        var resizedImage = image.Clone();
                        resizedImage.Resize((uint)size, (uint)size);
                        resizedImage.Format = MagickFormat.Ico;
                        collection.Add(resizedImage);
                    }
                }

                await collection.WriteAsync(outputPath);
            }
        }

        private async Task ConvertSvgToIcoAsync(Stream inputStream, string outputPath, List<int> sizes)
        {
            using (var collection = new MagickImageCollection())
            {
                foreach (var size in sizes.OrderBy(s => s))
                {
                    using (var image = new MagickImage())
                    {
                        inputStream.Position = 0; // Reset stream position
                        
                        // Configurar densidade para SVG
                        image.Density = new Density(300);
                        image.BackgroundColor = MagickColors.Transparent;
                        
                        await image.ReadAsync(inputStream, MagickFormat.Svg);
                        image.Resize((uint)size, (uint)size);
                        image.Format = MagickFormat.Ico;
                        
                        collection.Add(image.Clone());
                    }
                }

                await collection.WriteAsync(outputPath);
            }
        }

        public void CleanupTempFiles(TimeSpan olderThan)
        {
            var tempPath = Path.Combine(_webHostEnvironment.WebRootPath, "temp");
            if (!Directory.Exists(tempPath))
                return;

            var cutoffTime = DateTime.Now - olderThan;
            var files = Directory.GetFiles(tempPath, "*.ico");

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                if (fileInfo.CreationTime < cutoffTime)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        // Ignorar erros ao deletar arquivos temporários
                    }
                }
            }
        }
    }
}