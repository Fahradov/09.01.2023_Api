using System;
namespace StoreApi.Helpers
{
	public class FileManager
	{
        public static string Save(IFormFile file, string rootpath, string folder)
        {
            string fileName = file.FileName;
            string newFileName = Guid.NewGuid().ToString() + (fileName.Length > 64 ? fileName.Substring(fileName.Length - 64) : fileName);
            string path = Path.Combine(rootpath, folder, newFileName);
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fs);
            }
            return newFileName;
        }
        public static void Delete(string rootpath, string folder, string fileName)
        {
            string path = Path.Combine(rootpath, folder, fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}

