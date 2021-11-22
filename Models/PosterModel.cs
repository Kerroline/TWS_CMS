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
        

        public int mangaID { get; set; }

        public PosterModel()
        { }
        public PosterModel(string mangaName, IFormFile image) 
            : base(mangaName, image)
        {
        }     
    }
}
