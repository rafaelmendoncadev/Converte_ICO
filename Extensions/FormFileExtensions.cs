namespace Converte_ICO.Extensions
{
    public static class FormFileExtensions
    {
        public static async Task<byte[]> GetBytesAsync(this IFormFile formFile)
        {
            if (formFile == null)
                return Array.Empty<byte>();

            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}