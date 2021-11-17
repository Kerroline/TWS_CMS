using MangaCMS.DAL;
using MangaCMS.Models;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        readonly MangaCMSContext _MangaContext;
        public ChapterController(MangaCMSContext context)
        {
            _MangaContext = context;
        }

        /// <summary>
        /// Get all chapters for current manga.
        /// </summary>
        /// <param name="MangaID"></param>
        /// <returns>List Charpters of this Manga</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /GetForManga/1
        ///     {
        ///        "MangaID" : 1
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the list chapters</response>
        /// /// <response code="204">If chapters is null</response>
        /// <response code="400">If the manga not found</response>
        [Route("GetForManga")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<ChapterModel>> GetAllFromManga(int MangaID)
        {
            var current_manga = _MangaContext.Mangas.Find(MangaID);
            if (current_manga is not null)
            {
                var chapters = _MangaContext.Chapters.Where(ch => ch.MangaId == MangaID);

                if(chapters is not null)
                {
                    return StatusCode(200, chapters.ToList());
                }
                else
                {
                    return StatusCode(204, "The chapters not found");
                }
                
            }
            return StatusCode(400, "The Manga not found");
        }


        /// <summary>
        /// Creates a new Project.
        /// </summary>
        /// <param name="MangaID"></param>
        /// <param name="Chapter"></param>
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
        /// <response code="204">If successful created </response>
        /// <response code="400">If the model invalid or manga not found</response>
        [Route("Create")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ChapterModel>> CreateChapter(int MangaID, ChapterModel Chapter)
        {
            if (ModelState.IsValid && MangaID != 0)
            {

                var MangasExist = _MangaContext.Mangas.Find(MangaID);
                if (MangasExist is not null)
                {
                    //var ChrNameWithoutSpaces = Chapter.ChapterName.Replace(" ", "");
                    //MangasExist.ContentDirPath;

                    //if (!Directory.Exists(Manga.ContentDirPath))
                    //{
                    //    Directory.CreateDirectory(Manga.ContentDirPath);
                    //}
                    //if (Directory.Exists(Manga.ContentDirPath))
                    //{



                    //    _db.Mangas.Add(Manga);
                    //    await _db.SaveChangesAsync();


                    List<ChapterModel> chapter_list = await _MangaContext.Chapters.ToListAsync();
                    var created_chapter = chapter_list.Last();

                    if ((created_chapter.ChapterName == Chapter.ChapterName)
                        &&
                        (created_chapter.ChapterNumber == Chapter.ChapterNumber)
                        &&
                        (created_chapter.MangaId == Chapter.MangaId))
                    {
                        return StatusCode(204);
                    }
                    else
                    {
                        return StatusCode(500, "Chapter fail to created");
                    }
                //}
                //else
                //{
                //    return statuscode(500, "content directory not found");
                //}
                }
                else
                {
                    return StatusCode(400, "A Manga not found");
                }
            //    var EngNameWithoutSpaces = Manga.ENG_Name.Replace(" ", "");


            //    Manga.ContentDirPath = _env.WebRootPath + "/Content/Manga/" + EngNameWithoutSpaces;
            //    if (!Directory.Exists(Manga.ContentDirPath))
            //    {
            //        Directory.CreateDirectory(Manga.ContentDirPath);
            //    }
            //    if (Directory.Exists(Manga.ContentDirPath))
            //    {
            //        if (Manga.StatusId == 0)
            //        {
            //            Manga.StatusId = 1;
            //        }
            //        _db.Mangas.Add(Manga);
            //        await _db.SaveChangesAsync();
            //        List<MangaModel> manga_list = await _db.Mangas.ToListAsync();
            //        var created_manga = manga_list.Last();

            //        if (created_manga.ENG_Name == Manga.ENG_Name)
            //        {
            //            return StatusCode(201, created_manga.Id);
            //        }
            //        else
            //        {
            //            return StatusCode(500, "Manga fail to created");
            //        }
            //    }
            //    else
            //    {
            //        return StatusCode(500, "Content Directory not found");
            //    }
            //}
            //else
            //{
            //    return StatusCode(400, "A project already exists with this name");
            //}
        }
        return StatusCode(400, "Invalid model");

    }
    }
}
