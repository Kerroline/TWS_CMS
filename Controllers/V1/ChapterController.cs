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
        /// <param name="id"></param>
        /// <returns>List Charpters of this Manga</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /GetOnce
        ///     {
        ///        "MangaID" : 1
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the list chapters</response>
        /// /// <response code="204">If chapters is null</response>
        /// <response code="400">If the manga not found</response>
        [Route("GetOnce")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<ChapterModel>> GetOnceChapter(int id)
        {
            var currentChapter = await _mangaContext.Chapters.FindAsync(id);
            if (currentChapter is not null)
            {
                return currentChapter;
            }
            return StatusCode(400, "The Chapter is not found");
        }

        /// <summary>
        /// Get all chapters for current manga.
        /// </summary>
        /// <param name="MangaID"></param>
        /// <returns>List Charpters of this Manga</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /GetAllForManga
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

                    return StatusCode(201);
                }
                else
                {
                    return StatusCode(400, "A Manga not found");
                }
            }
            return StatusCode(400, "Invalid model");
        }



        // -----------------------------------------------------------------





        /// <summary>
        /// Edit a exist chapter
        /// </summary>
        /// <param name="chapter"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Edit/1
        ///     {
        ///        "StatusName": "newName"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">If the chapter was updated</response>
        /// <response code="400">If the chapter is not found or model invalid</response>
        [Route("Edit/{id}")]
        [HttpPut]
        public async Task<ActionResult<StatusModel>> Edit(int id, ChapterModel newChapter)
        {
            if (ModelState.IsValid)
            {
                var oldChapter = await _mangaContext.Chapters.FindAsync(id);
                if (oldChapter is not null)
                {
                    oldChapter.ChapterNumber = newChapter.ChapterNumber;
                    oldChapter.ChapterName = newChapter.ChapterName;
                    oldChapter.MangaId = newChapter.MangaId;

                    await _mangaContext.SaveChangesAsync();
                    return StatusCode(200, "A Chapter was updated");
                }
                return StatusCode(400, "A chapter is not found");
            }
            return StatusCode(400, "Model invalid");
        }

        /// <summary>
        /// Delete exist chapter if it has not pages
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Delete/1
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <response code="204">If the chapter was deleted</response>
        /// <response code="400">If the chapter has pages or not founded</response>
        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<ActionResult<StatusModel>> Delete(int id)
        {
            var currentChapter = 
                await _mangaContext.Chapters.Include(ch => ch.Pages)
                                            .SingleOrDefaultAsync(ch => ch.Id == id);

            if (currentChapter is not null)
            {
                if(currentChapter.Pages.Any())
                {
                    return StatusCode(400, "A Chapter includes Pages");
                } 
                else
                {
                    _mangaContext.Chapters.Remove(currentChapter);
                    await _mangaContext.SaveChangesAsync();
                    return StatusCode(204, "A Chapter was deleted");
                }
                
            }
            return StatusCode(400, "A Chapter not found");
        }
    }
}
