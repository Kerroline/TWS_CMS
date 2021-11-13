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
    public class MangaController : ControllerBase
    {
        private readonly MangaCMSContext _db;
        private readonly IWebHostEnvironment _env;
        public MangaController(MangaCMSContext context, IWebHostEnvironment env)
        {
            _db = context;
            _env = env;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MangaModel>>> GetAll()
        {
            var mangas = _db.Mangas.Include(m => m.Status);
            return await mangas.ToListAsync();
        }

        [Route("Create")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MangaModel>> CreateMangaWithoutPoster(MangaModel Manga)
        {
            if (ModelState.IsValid)
            {
                if (Manga.ENG_Name != null)
                {
                    var MangasExist = _db.Mangas.FirstAsync
                        (m => ((m.ENG_Name == Manga.ENG_Name) ||
                               (m.RU_Name == Manga.RU_Name) ||
                               (m.JP_Name == Manga.JP_Name))
                        );
                    if (MangasExist == null)
                    {
                        var EngNameWithoutSpaces = Manga.ENG_Name.Replace(" ", "");

                        Manga.ContentDirPath = _env.WebRootPath + "/Content/Manga/" + EngNameWithoutSpaces;
                        if (!Directory.Exists(Manga.ContentDirPath))
                        {
                            Directory.CreateDirectory(Manga.ContentDirPath);
                        }
                        if (Directory.Exists(Manga.ContentDirPath))
                        {
                            _db.Mangas.Add(Manga);
                            await _db.SaveChangesAsync();
                            List<MangaModel> manga_list = await _db.Mangas.ToListAsync();
                            var manga_id = manga_list.Last().Id;

                            if (manga_id != null)
                            {
                                return StatusCode(201,manga_id);
                            }
                            else
                            {
                                return BadRequest("Manga fail to created");
                            }
                            
                        }
                        else
                        {
                            return BadRequest("Manga Dir fail to created");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(Manga.ENG_Name, "A project already exists with this name");
                        return BadRequest(ModelState);
                    }

                }
            }
            return BadRequest("Invalid model");
            
        }

        [Route("Create/{id}/Poster")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<IFormFile> UploadPosterToManga(int id, IFormFile Poster)
        {
            if (Poster != null)
            {
                var current_manga = _db.Mangas.Find(id);
                if (current_manga != null)
                {
                    current_manga.PosterPath = current_manga.ContentDirPath + "/Posters";
                    if (!Directory.Exists(current_manga.PosterPath))
                    {
                        Directory.CreateDirectory(current_manga.PosterPath);
                    }
                    if(Directory.Exists(current_manga.PosterPath))
                    {
                        using (var fileStream = new FileStream(current_manga.PosterPath, FileMode.Create))
                        {
                            Poster.CopyTo(fileStream);
                        }
                        if (System.IO.File.Exists(current_manga.PosterPath + Poster.FileName))
                        {
                            return StatusCode(201);
                        }
                        else
                        {
                            return BadRequest("Image not upload");
                        }
                    }
                    else
                    {
                        return BadRequest("Manga Dir not found");
                    }
                }
                return BadRequest("The Manga not exist");
            }
            return BadRequest("Image not sended");
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
