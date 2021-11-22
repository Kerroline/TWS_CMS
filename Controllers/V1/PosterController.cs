using MangaCMS.DAL;
using MangaCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PosterController : ControllerBase
    {
        private readonly MangaCMSContext _mangaContext;
        private readonly IWebHostEnvironment _env;
        public PosterController(MangaCMSContext context, IWebHostEnvironment env)
        {
            _mangaContext = context;
            _env = env;
        }

        /// <summary>
        /// Upload Poster to Manga.
        /// </summary>
        /// <param name="mangaID">Manga ID in URL</param>
        /// <param name="image">Poster ID </param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Upload
        ///     {
        ///        "mangaID": 1
        ///        "image": file.png
        ///     }
        ///
        /// </remarks>
        /// <response code="204">If poster upload</response>
        /// <response code="400">If poster not sended or manga not found</response>
        [Route("Upload")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<IFormFile> UploadPosterToManga(int mangaID, IFormFile image)
        {
            if (image != null)
            {
                var current_manga = _mangaContext.Mangas.Find(mangaID);
                if (current_manga is not null)
                {
                    string mangaNameWithoutSpaces = current_manga.englishName.Replace(" ", "").ToLower();

                    PosterModel poster = new PosterModel(mangaNameWithoutSpaces, image)
                    {
                        mangaID = current_manga.ID
                    };

                    string pathToSave = Path.Combine(_env.WebRootPath, poster.filePath);
                    poster.UploadFile(pathToSave, image);

                    
                    if (poster.IsUpload(pathToSave))
                    {
                        current_manga.listOfPosters.Add(poster);
                        _mangaContext.Add(poster);
                        _mangaContext.SaveChanges();

                        return StatusCode(204);
                    }
                    else
                    {
                        return StatusCode(500, "Image not upload");
                    }
                }
                return StatusCode(400, "The Manga not exist");
            }
            return StatusCode(400, "Image not sended");
        }

    }
}
