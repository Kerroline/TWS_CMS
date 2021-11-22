using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MangaCMS.Models
{
    public class FileModel
    {
        [JsonIgnore]
        [Key]
        public int ID { get; set; }

        [JsonIgnore]
        public string fileName { get; set; }

        [JsonIgnore]
        public string filePath { get; set; }

        public FileModel(string mangaName,IFormFile file)
        {
            fileName = GenerateUniqueFilename(Path.GetExtension(file.FileName));
            filePath = $"Content/Manga/{mangaName}/Posters/{fileName}";
        }
        public FileModel(string mangaName,double chapterNumber, IFormFile file)
        {
            fileName = GenerateUniqueFilename(Path.GetExtension(file.FileName));
            filePath = $"Content/Manga/{mangaName}/{chapterNumber}/{fileName}";
        }
        public FileModel()
        {

        }



        public void UploadFile(string pathToSave, IFormFile file)
        {
            using (var fileStream = new FileStream(pathToSave, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }
        public bool IsUpload(string pathToSave)
        {
            var current_files = Directory.GetFiles(pathToSave, "*", SearchOption.AllDirectories);
            foreach (var file in current_files)
            {
                if (file == pathToSave)
                {
                    return true;
                }
            }
            return false;
        }

        protected bool IsDirectoryExist(string path)
        {
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (Directory.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected string GenerateUniqueFilename(string extension)
        {
            return $"{Guid.NewGuid().ToString().Replace("-", "")}{extension}";
        }
    }
}
