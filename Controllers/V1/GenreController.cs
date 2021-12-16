using MangaCMS.DAL;
using MangaCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.Controllers.V1
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly MangaCMSContext _mangaContext;
        public GenreController(MangaCMSContext context)
        {
            _mangaContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreModel>>> GetAll()
        {
            return await _mangaContext.Genres.ToListAsync();
        }


        /// <summary>
        /// Creates a new Genre.
        /// </summary>
        /// <param name="Genre"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Create
        ///     {
        ///        "GenreName": "Name"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">If the genre is created</response>
        /// <response code="400">If the genre is exist or model invalid</response>
        [Route("Create")]
        [HttpPost]
        public async Task<ActionResult<GenreModel>> Create(GenreModel Genre)
        {
            if (ModelState.IsValid)
            {
                foreach (var genres in _mangaContext.Genres)
                {
                    if (genres.GenreName == Genre.GenreName)
                    {
                        return StatusCode(400, "Genre exist");
                    }
                }
                _mangaContext.Genres.Add(Genre);
                await _mangaContext.SaveChangesAsync();

                List<GenreModel> genre_list = await _mangaContext.Genres.ToListAsync();
                var created_genres = genre_list.Last();

                if (created_genres.GenreName == Genre.GenreName)
                {
                    return StatusCode(204, "Genre created");
                }
                else
                {
                    return StatusCode(500, "Status fail to created");
                }
            }
            return StatusCode(400, "Model invalid");
        }


    }
}
