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
    public class PagesController : ControllerBase
    {
        private readonly MangaCMSContext _mangaContext;
        private readonly IWebHostEnvironment _env;
        public PagesController(MangaCMSContext context, IWebHostEnvironment env)
        {
            _mangaContext = context;
            _env = env;
        }


        /// <summary>
        /// Upload pages (images) to chapter
        /// </summary>
        /// <param name="chapterID">Chapter ID</param>
        /// <param name="pages">Collection of images </param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Create
        ///     {
        ///        "chapterID": 1
        ///        "pages" []: file.png,file.png, ... file.png
        ///     }
        ///
        /// </remarks>
        /// <response code="204">If poster upload</response>
        /// <response code="400">If poster not sended or manga not found</response>
        [Route("Create")]
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult CreatePages([FromForm]int chapterID, IFormFileCollection pages)
        {
            if (pages.Count > 0)
            {
                var current_chapters = _mangaContext.Chapters.Find(chapterID);
                if (current_chapters is not null)
                {
                    string curMangaName = string.Empty;
                    var chapterNumber = current_chapters.ChapterNumber;
                    var mangas = _mangaContext.Mangas.Where(m => m.ID == current_chapters.MangaId);
                    if(mangas.Count() == 1)
                    {
                        curMangaName = mangas.Single().englishName.Replace(" ", "").ToLower();
                    }

                    int pageNumber = 1;
                    foreach (var pageFile in pages)
                    {
                        FileModel file = new FileModel(curMangaName, chapterNumber, pageFile);

                        
                        _mangaContext.Files.Add(file);
                        _mangaContext.SaveChanges();

                        PageModel page = new()
                        {
                            ChapterId = chapterID,
                            PageNumber = pageNumber,
                            FileId = file.ID,
                        };
                        _mangaContext.Pages.Add(page);

                        string pathToSave = Path.Combine(_env.WebRootPath, file.filePath);
                        file.UploadFile(pathToSave, pageFile);
                        pageNumber++;
                    }
                    _mangaContext.SaveChanges();
                    return StatusCode(201);

                }
                return StatusCode(400, "The Chapter not exist");
            }
            return StatusCode(400, "Pages not sended");
        }
    }
}
