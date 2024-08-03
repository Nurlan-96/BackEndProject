namespace WebApplication1.Extensions
{
    public static class Extension
    {
        public static bool CheckContentType(this IFormFile file)
        {
            return file.ContentType.Contains("image/");
        }
        public static bool CheckSize(this IFormFile file, int size)
        {
            return file.Length / 1024 > size;
        }        
        public static async Task<string> SaveFile(this IFormFile file)
        {
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);
            using FileStream fileStream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return fileName;
        }
    }
}
