using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string[] AllowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        private readonly long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AttachmentService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string? Upload(string FolderName, IFormFile File)
        {
            try
            {
                //1 check size
                if (FolderName is null || File is null || File.Length == 0) return null;
                if (File.Length > MaxFileSize) return null;

                //2 check extension
                var Extension = Path.GetExtension(File.FileName).ToLower();
                if (!AllowedExtensions.Contains(Extension)) return null;


                //3 Get Folder Path              //D:\Csharp projects\GymManagementSystemSolution\GymManagementPL\ => Current Dir
                //var FolderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","images",FolderName);
                var FolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", FolderName); //wwwroot here is the current dir 

                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }

                //4 Create Unique File Name
                //a7sd5vss1WSD6v6.jpg
                var FileName = Guid.NewGuid().ToString() + Extension;

                //5 Get file full path
                var FilePath = Path.Combine(FolderPath, FileName);

                //6 Copy File to target location
                using var FileStream = new FileStream(FilePath, FileMode.Create); // to add file to that path - auto dispose

                File.CopyTo(FileStream);

                return FileName; // or FilePath if u want full path (I think it is the easiest way)
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Failed To Upload File To Folder = {FolderName} : {ex}");
                return null;
            }
        }
        public bool Delete(string FileName, string FolderName)
        {
            //1 
            try
            {
                if (string.IsNullOrEmpty(FileName) || string.IsNullOrEmpty(FolderName)) return false;
                var FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", FolderName, FileName);
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Delete File {FileName} From Folder = {FolderName} : {ex}");
                return false;
            }

        }
    }
}
