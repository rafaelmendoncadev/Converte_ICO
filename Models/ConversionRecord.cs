using System;

namespace Converte_ICO.Models
{
    public class ConversionRecord
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string ConvertedFileName { get; set; } = string.Empty; // stored filename in wwwroot/temp
        public string Sizes { get; set; } = string.Empty; // e.g. "16,32,64"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
