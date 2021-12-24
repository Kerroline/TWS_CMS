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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MangaCMS.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MangaController : ControllerBase
    {
        private readonly MangaCMSContext _mangaContext;
        private readonly IWebHostEnvironment _env;
        public MangaController(MangaCMSContext context, IWebHostEnvironment env)
        {
            _mangaContext = context;
            _env = env;
        }

        /// <summary>
        /// Get all mangas
        /// </summary>
        /// <returns>All mangas in database</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /GetAll
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <response code="200">List of Mangas</response>
        [Route("GetAll")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MangaModel>>> GetAll()
        {
            return await _mangaContext.Mangas.Include(m => m.status)
                                             .Include(m => m.listOfGenres)
                                             .Include(m => m.listOfPosters)
                                             .ToListAsync();
        }

        /// <summary>
        /// Get once manga.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Get/1
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <response code="200">A manga by id</response>
        /// <response code="400">If a manga is not found</response>
        [Route("Get/{id}")]
        [HttpGet]
        public async Task<ActionResult<MangaModel>> GetByID(int mangaID)
        {
            var allMangas = await _mangaContext.Mangas.Include(m => m.status)
                                             .Include(m => m.listOfGenres)
                                             .Include(m => m.listOfPosters)
                                             .ToListAsync();

            var currentManga = allMangas.Find(m => m.ID == mangaID);

            if (currentManga is not null)
            {
                return StatusCode(200,currentManga);
            }
            return StatusCode(400, "The manga is not found");
        }
        /// <summary>
        /// Creates a new Project.
        /// </summary>
        /// <param name="Manga"></param>
        /// <returns>A newly created project ID</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Create
        ///     {
        ///         "japanName": "Kore wa hon",
        ///         "russianName": "Это книга",
        ///         "englishName": "This is book",
        ///         "link": "http.readmanga.",
        ///         "year": 2000,
        ///         "author": "Watakushi",
        ///         "description": "This story ... ",
        ///         "statusID": 1,
        ///     }        
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item ID</response>
        /// <response code="400">If the project is exist</response>
        [Route("Create")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MangaModel>> CreateMangaWithoutPoster(MangaModel Manga)
        {
            if (ModelState.IsValid)
            {
                var MangasExist = _mangaContext.Mangas.Where
                    (m => ((m.englishName == Manga.englishName) ||
                            (m.russianName == Manga.russianName) ||
                            (m.japanName == Manga.japanName))
                    );
                if (!MangasExist.Any())
                {
                    if (Manga.statusID == 0)
                    {
                        Manga.statusID = 1;
                    }

                    _mangaContext.Mangas.Add(Manga);
                    await _mangaContext.SaveChangesAsync();
                    return StatusCode(201, Manga.ID);
                }
                else
                {
                    return StatusCode(400, "A project already exists with this name");
                }
            }
            return StatusCode(400, "Invalid model");    
        }


        /// <summary>
        /// Edit a exist status
        /// </summary>
        /// <param name="mangaID">Manga ID in URL</param>
        /// <param name="newManga">Manga with new field value</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Edit/1
        ///     {
        ///         "japanName": "Kore wa hon",
        ///         "russianName": "Это книга",
        ///         "englishName": "This is book",
        ///         "link": "http.readmanga.",
        ///         "year": 2000,
        ///         "author": "Watakushi",
        ///         "description": "This story ... ",
        ///         "statusID": 1,
        ///     }
        ///
        /// </remarks>
        /// <response code="200">If the manga was updated</response>
        /// <response code="400">If a manga is not found or model invalid</response>
        [Authorize("Admin")]
        [Route("Edit/{id}")]
        [HttpPut]
        public async Task<ActionResult<StatusModel>> Edit(int mangaID, MangaModel newManga)
        {
            var oldManga = await _mangaContext.Mangas.FindAsync(mangaID);
            if (oldManga is not null)
            {
                if (ModelState.IsValid)
                {
                    oldManga.japanName = newManga.japanName;
                    oldManga.russianName = newManga.russianName;
                    oldManga.englishName = newManga.englishName;
                    oldManga.link = newManga.link;
                    oldManga.author = newManga.author;
                    oldManga.description = newManga.description;
                    oldManga.statusID = newManga.statusID;

                    await _mangaContext.SaveChangesAsync();
                    return StatusCode(200, "A Manga was updated");
                }
            }
            return StatusCode(400, "A manga is not found or model invalid");
        }


        /// <summary>
        /// Delete an exist manga
        /// </summary>
        /// <param name="mangaID">MangaID in URL</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /Delete/1
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <response code="204">If the manga was deleted</response>
        /// <response code="400">If the manga is not found</response>
        [Authorize("Admin")]
        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<ActionResult<MangaModel>> Delete(int mangaID)
        {
            var currentManga = await _mangaContext.Mangas.FindAsync(mangaID);
            if (currentManga is not null)
            {
                _mangaContext.Mangas.Remove(currentManga);
                await _mangaContext.SaveChangesAsync();
                return StatusCode(200, "A manga was deleted successfully ");
            }
            return StatusCode(400, "A manga has not found");
        }





        //if (Manga.EngName != null)
        //{
        //    foreach (var mang in _db.Mangas)
        //    {
        //        if ((mang.EngName == Manga.EngName) ||
        //            (mang.RuName == Manga.RuName) ||
        //            (mang.OrigName == Manga.OrigName))
        //        {
        //            ModelState.AddModelError("MangaName", "A project already exists with this name");
        //            return BadRequest(ModelState);
        //        }
        //    }
        //    _db.Mangas.Add(Manga);
        //    await _db.SaveChangesAsync();
        //    List<MangaModel> manga_list = await _db.Mangas.ToListAsync();
        //    var manga_id = manga_list.Last().Id;
        //    return StatusCode(201, manga_id);
        //}


        //if (Genre.GenreName != null)
        //{
        //    foreach (var genr in _db.Genres)
        //    {
        //        if (genr.GenreName == Genre.GenreName)
        //        {
        //            ModelState.AddModelError("GenreName", "Жанр уже существует");
        //            return BadRequest(ModelState);
        //        }
        //    }
        //    _db.Genres.Add(Genre);
        //    await _db.SaveChangesAsync();
        //    return StatusCode(201);
        //}
        //return StatusCode(400);

        /*
         *  if (ModelState.IsValid)
        {
            // Path to poster for this manga
            // ~wwwroot/Content/Manga/ "EngName" /Posters 

            var EngNameWithoutSpaces = mangaCreate.Manga.EngName.Replace(" ","");
            string path_to_poster = "Content/Manga/" + EngNameWithoutSpaces + "/Posters";

            string path_to_Create_folder = _appEnvironment.WebRootPath + "/" + path_to_poster;
            if (!Directory.Exists(path_to_Create_folder))
            {
                Directory.CreateDirectory(path_to_Create_folder);
            }

            string path_to_upload_image = path_to_Create_folder + "/" + mangaCreate.Manga.Poster;
            path_to_poster += "/" + mangaCreate.Manga.Poster;
            mangaCreate.Manga.Poster = path_to_poster;

            using (var fileStream = new FileStream(path_to_upload_image, FileMode.Create))
            {
                mangaCreate.File.CopyTo(fileStream);
            }

            _db.Mangas.Add(mangaCreate.Manga);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
         */
    }
}
