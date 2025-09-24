using System.ComponentModel.DataAnnotations;

namespace Converte_ICO.Models
{
    public class ImageUploadViewModel
    {
        [Required(ErrorMessage = "Por favor, selecione uma imagem.")]
        public IFormFile? ImageFile { get; set; }

        public string? ImagePreviewUrl { get; set; }
        
        public List<int> SelectedSizes { get; set; } = new List<int>();
        
        public bool Size16 { get; set; }
        public bool Size32 { get; set; }
        public bool Size64 { get; set; }
        public bool Size128 { get; set; }
        
        public string? ConvertedImagePath { get; set; }
        public bool IsConverted { get; set; }
        
        public string CustomFileName { get; set; } = "icon";
    }
}