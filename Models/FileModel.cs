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
        public string Path { get; set; }

        [JsonIgnore]
        readonly string rootpath; 

        public FileModel(string filename, string mangaName,IFormFile file, IWebHostEnvironment env)
        {
            rootpath = $"{env.WebRootPath}/Content/Manga/";
            string pathToSave = $"{rootpath}{mangaName}/Posters/";
            //var s1 = IsDirectoryExist(basePath);
            //var s2 = IsDirectoryExist(pathToSave);
            UploadFile(pathToSave, file, filename);
            var s3 = IsUpload(filename);
            Path = $"/Content/Manga/{mangaName}/Posters/{filename}";
        }
        public FileModel()
        {

        }



        protected void UploadFile(string pathToSave, IFormFile file, string filename)
        {

            if(IsDirectoryExist(pathToSave))
            {
                using (var fileStream = new FileStream($"{pathToSave}/{filename}", FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
        }
        public bool IsUpload(string filename)
        {
            var current_files = Directory.GetFiles(rootpath, "*", SearchOption.AllDirectories);
            foreach(var file in current_files)
            {
                if(file == filename)
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

        protected static string GenerateUniqueFilename(string extension)
        {
            return $"{Guid.NewGuid()}{extension}";
        }
    }
}
