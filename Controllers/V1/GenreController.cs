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

        /// <summary>
        /// Get all genres
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /GetAll
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <response code="200">List of Genres</response>
        [Route("GetAll")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreModel>>> GetAll()
        {
            return await _mangaContext.Genres.ToListAsync();
        }

        /// <summary>
        /// Get once genre.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /GetOnce/1
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Genre by id</response>
        /// <response code="400">Genre is not found</response>
        [Route("GetOnce")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreModel>> GetOnce(int id)
        {
            var currentGenre = await _mangaContext.Genres.FindAsync(id);
            if (currentGenre is not null)
            {
                return currentGenre;
            }
            return StatusCode(400, "Genre is not found");
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

                return StatusCode(204, "Genre created");
            }
            return StatusCode(400, "Model invalid");
        }

        /// <summary>
        /// Edit an exist genre
        /// </summary>
        /// <param name="id"></param>
        /// <param name="genre"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Edit/1
        ///     {
        ///        "GenreName": "newName"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">If the genre is update</response>
        /// <response code="400">If the genre is not found</response>
        [Route("Edit")]
        [HttpPut("{id}")]
        public async Task<ActionResult<GenreModel>> Edit(int id, GenreModel genre)
        {
            var currentGenre = await _mangaContext.Genres.FindAsync(id);
            if (currentGenre is not null)
            {
                if (ModelState.IsValid)
                {
                    currentGenre.GenreName = genre.GenreName;
                    await _mangaContext.SaveChangesAsync();
                    return StatusCode(200, "A GenreName is update");
                }
            }
            return StatusCode(400, "A Genre is not found");
        }

        /// <summary>
        /// Delete an exist genre
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
        /// <response code="204">If the status is deleted</response>
        /// <response code="400">If the status is not found</response>
        [Route("Delete")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<GenreModel>> Delete(int id)
        {
            var currentGenre = await _mangaContext.Genres.FindAsync(id);
            if (currentGenre is not null)
            {
                _mangaContext.Genres.Remove(currentGenre);
                await _mangaContext.SaveChangesAsync();
                return StatusCode(200, "A Genre deleted successfully");
            }
            return StatusCode(400, "A Genre is not found");
        }


    }
}
