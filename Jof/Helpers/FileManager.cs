namespace Jof.Helpers
{
    public static class FileManager
    {
        public static async Task<string> UploadImage(this IFormFile Image , string env,string folderName)
        {
            string path = env+folderName;
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName=Image.FileName;
            fileName = Guid.NewGuid().ToString()+fileName;
            path = env+folderName+fileName;
            using(FileStream stream = new FileStream(path, FileMode.Create))
            {
                await Image.CopyToAsync(stream);
            }
            return fileName;
        }
        public static bool CheckImage(this IFormFile Image,int size)
        {
            return Image.ContentType.Contains("image/") && Image.Length/1024/1024 < size;
        }
    }
}
