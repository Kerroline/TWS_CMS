using MangaCMS.DAL;
using MangaCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
                //string mangaNameWithoutSpaces = current_manga.englishName.Replace(" ", "");
                //string mangaNameWithoutSpaces = "TESTMANGAAAAAAAAAAAA";
                //PosterModel poster = new PosterModel(mangaNameWithoutSpaces, image, _env);

                var current_manga = _mangaContext.Mangas.Find(mangaID);
                if (current_manga is not null)
                {
                    

                    //poster.IsUpload()

                    if (true)
                    {
                        string mangaNameWithoutSpaces = current_manga.englishName.Replace(" ", "");
                        PosterModel poster = new PosterModel(mangaNameWithoutSpaces, image, _env)
                        {
                            mangaID = current_manga.ID
                        };
                        current_manga.listOfPosters.Add(poster);
                        _mangaContext.Add(poster);
                        _mangaContext.SaveChanges();

                        return StatusCode(204);
                    }
                    else
                    {
                        return StatusCode(500, "Image not upload");
                    }
                    //        current_manga.PosterPath = current_manga.ContentDirPath + "/Posters";
                    //        if (!Directory.Exists(current_manga.PosterPath))
                    //        {
                    //            Directory.CreateDirectory(current_manga.PosterPath);
                    //        }
                    //        if(Directory.Exists(current_manga.PosterPath))
                    //        {
                    //            int PosterCount = Directory.GetFiles(current_manga.PosterPath, "*", SearchOption.TopDirectoryOnly).Length;
                    //            string PosterName = $"Poster{PosterCount}.png";
                    //            var PathToSave = Path.Combine(current_manga.PosterPath + "/" + PosterName);
                    //            using (var fileStream = new FileStream(PathToSave, FileMode.Create))
                    //            {
                    //                Poster.CopyTo(fileStream);
                    //            }
                    //            if (System.IO.File.Exists(current_manga.PosterPath + "/" + PosterName))
                    //            {
                    //                return StatusCode(204);
                    //            }
                    //            else
                    //            {
                    //                return StatusCode(500, "Image not upload");
                    //            }
                    //        }
                    //        else
                    //        {
                    //            return StatusCode(500, "Manga Directory not found");
                    //        }
                }
                return StatusCode(400, "The Manga not exist");
            }
            return StatusCode(400, "Image not sended");
        }

    }
}
