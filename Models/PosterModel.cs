using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.Models
{
    public class PosterModel : FileModel
    {
        public string posterName { get; set;}

        public int mangaID { get; set; }

        public PosterModel()
        { }
        public PosterModel(string mangaName, IFormFile image, IWebHostEnvironment env) 
            : base(GenerateName(out string posterName), mangaName, image, env)
        {
            this.posterName = posterName;
        }

        private static string GenerateName(out string posterName)
        {
            return posterName = GenerateUniqueFilename(".png");
        }       

    }
}
