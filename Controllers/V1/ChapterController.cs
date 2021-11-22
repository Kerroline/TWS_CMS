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
        private readonly MangaCMSContext _mangaContext;
        public ChapterController(MangaCMSContext context)
        {
            _mangaContext = context;
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
        [Route("GetAllForManga")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<ChapterModel>> GetAllFromManga(int MangaID)
        {
            var current_manga = _mangaContext.Mangas.Find(MangaID);
            if (current_manga is not null)
            {
                var chapters = _mangaContext.Chapters.Where(ch => ch.MangaId == MangaID);

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
        public async Task<ActionResult<ChapterModel>> CreateChapter(ChapterModel Chapter)
        {
            if (ModelState.IsValid)
            {

                var MangasExist = _mangaContext.Mangas.Find(Chapter.MangaId);
                if (MangasExist is not null)
                {
                    MangasExist.listOfChapters.Add(Chapter);

                    _mangaContext.Chapters.Add(Chapter);
                    await _mangaContext.SaveChangesAsync();

                    List<ChapterModel> chapter_list = await _mangaContext.Chapters.ToListAsync();
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
                }
                else
                {
                    return StatusCode(400, "A Manga not found");
                }
            }
            return StatusCode(400, "Invalid model");
        }
    }
}
