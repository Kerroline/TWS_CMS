using MangaCMS.DAL;
using MangaCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        /// Get all upload poster to the current manga
        /// </summary>
        /// <param name="mangaID">Manga ID in URL</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /GetAll/1
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <response code="200">List of String path to poster</response>
        /// <response code="400">If a manga is not found or has not posters</response>
        [Route("GetAll/{mangaID}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAll(int mangaID)
        {
            var allPosterForManga = await _mangaContext.Posters.Where(p => p.mangaID == mangaID).ToListAsync();

            if (allPosterForManga.Any())
            {
                string posterPath = allPosterForManga.Last().filePath;
                var allPostersPath = allPosterForManga.Select(p => p.filePath).ToList();
                
                return allPostersPath;
            }
            return StatusCode(400, "The manga has not posters or has not posters");
        }

        /// <summary>
        /// Get a poster by ID
        /// </summary>
        /// <param name="posterID">Poster ID in URL</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /GetByID/1
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <response code="200">List of String path to poster</response>
        /// <response code="400">If a manga is not found or has not posters</response>
        [Route("GetByID/{posterID}")]
        [HttpGet]
        public async Task<ActionResult<string>> GetByID(int posterID)
        {
            var currentPoster = await _mangaContext.Posters.FindAsync(posterID);

            if (currentPoster is not null)
            {
                return currentPoster.filePath;
            }
            return StatusCode(400, "The manga has not posters or has not posters");
        }

        /// <summary>
        /// Get last upload poster to the current manga
        /// </summary>
        /// <param name="mangaID">Manga ID in URL</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /GetLast/1
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <response code="200">String path to poster</response>
        /// <response code="400">If a manga is not found or has not posters</response>
        [Route("GetLast/{mangaID}")]
        [HttpGet]
        public async Task<ActionResult<string>> GetOnce(int mangaID)
        {
            var allPosterForManga = await _mangaContext.Posters.Where(p => p.mangaID == mangaID).ToListAsync();

            if(allPosterForManga.Any())
            {
                string posterPath = allPosterForManga.Last().filePath;
                return posterPath;
            }
            return StatusCode(400, "The manga is not found or has not posters");
        }

        /// <summary>
        /// Upload a poster to manga.
        /// </summary>
        /// <param name="mangaID">Manga ID in URL</param>
        /// <param name="image">Poster image</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Create
        ///     {
        ///        "mangaID": 1
        ///        "image": file.png
        ///     }
        ///
        /// </remarks>
        /// <response code="201">If a poster is upload</response>
        /// <response code="400">If poster not sended or manga not found</response>
        [Route("Create")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<IFormFile>> UploadPosterToManga([FromForm]int mangaID, IFormFile image)
        {
            if (image != null)
            {
                var current_manga = await _mangaContext.Mangas.FindAsync(mangaID);
                if (current_manga is not null)
                {
                    string mangaNameWithoutSpaces = current_manga.englishName.Replace(" ", "").ToLower();

                    PosterModel poster = new PosterModel(mangaNameWithoutSpaces, image)
                    {
                        mangaID = current_manga.ID
                    };

                    string pathToSave = Path.Combine(_env.WebRootPath, poster.filePath);
                    poster.UploadFile(pathToSave, image);


                    current_manga.listOfPosters.Add(poster);
                    _mangaContext.Add(poster);
                    await _mangaContext.SaveChangesAsync();

                    return StatusCode(201, "The poster is loaded successfully");

                }
                return StatusCode(400, "The Manga not exist");
            }
            return StatusCode(400, "Image not sended");
        }

        /// <summary>
        /// Change manga for this poster
        /// </summary>
        /// <param name="posterID">Poster ID in URL</param>
        /// /// <param name="mangaID">Poster image</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /EditByID/1
        ///     {
        ///        "mangaID": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="201">If a manga was changed</response>
        /// <response code="400">If poster is not found or manga is not found</response>
        [Route("EditByID/{posterID}")]
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<IFormFile>> EditByID(int posterID, int mangaID)
        {
            var currentPoster = await _mangaContext.Posters.FindAsync(posterID);
            if(currentPoster is not null)
            {
                var newManga = await _mangaContext.Mangas.FindAsync(mangaID);
                if(newManga is not null)
                {

                    currentPoster.mangaID = mangaID;
                    await _mangaContext.SaveChangesAsync();

                    return StatusCode(201, "A manga of this poster was changed successfully");
                }
                return StatusCode(400, "A manga is not found");
            }
            return StatusCode(400, "A poster is not found");
        }

        /// <summary>
        /// Delete an exist poster
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /Delete/1
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <response code="204">If the poster was deleted</response>
        /// <response code="400">If the poster is not found</response>
        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<ActionResult<PosterModel>> Delete(int id)
        {
            var currentPoster = await _mangaContext.Posters.FindAsync(id);
            if (currentPoster is not null)
            {
                _mangaContext.Posters.Remove(currentPoster);
                await _mangaContext.SaveChangesAsync();
                return StatusCode(204, "A Poster was deleted successfully");
            }
            return StatusCode(400, "A Poster is not found");
        }
    }
}
