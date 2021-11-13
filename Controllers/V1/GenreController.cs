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
        readonly MangaCMSContext _db;
        public GenreController(MangaCMSContext context)
        {
            _db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreModel>>> GetAll()
        {
            return await _db.Genres.ToListAsync();
        }

        [Route("Create")]
        [HttpPost]
        public async Task<ActionResult<GenreModel>> Create(GenreModel Genre)
        {
            if (Genre.GenreName != null)
            {
                foreach (var genr in _db.Genres)
                {
                    if (genr.GenreName == Genre.GenreName)
                    {
                        ModelState.AddModelError("GenreName", "Жанр уже существует");
                        return BadRequest(ModelState);
                    }
                }
                _db.Genres.Add(Genre);
                await _db.SaveChangesAsync();
                return StatusCode(201);
            }
            return StatusCode(400);
        }


    }
}
