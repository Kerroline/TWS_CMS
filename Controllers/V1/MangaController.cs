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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MangaModel>>> GetAll()
        {
            var mangas = _mangaContext.Mangas.Include(m => m.status);
            return await mangas.ToListAsync();
        }
        [Route("GetJSON")]
        [HttpGet]
        public ActionResult GetJson()
        {
            var mangas = _mangaContext.Mangas.Include(m => m.status);

            var mangass = mangas.ToList();

            //string json = JsonSerializer.Serialize(mangas);

            return Ok(mangass);
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
        ///        ...
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
                    List<MangaModel> manga_list = await _mangaContext.Mangas.ToListAsync();
                    var created_manga = manga_list.Last();

                    if (created_manga.englishName == Manga.englishName)
                    {
                        return StatusCode(201, created_manga.ID);
                    }
                    else
                    {
                        return StatusCode(500, "Manga fail to created");
                    }
                }
                else
                {
                    return StatusCode(400, "A project already exists with this name");
                }
            }
            return StatusCode(400, "Invalid model");    
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
