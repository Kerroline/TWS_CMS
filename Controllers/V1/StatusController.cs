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
    public class StatusController : ControllerBase
    {
        readonly MangaCMSContext _db;
        public StatusController(MangaCMSContext context)
        {
            _db = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusModel>>> GetAll()
        {
            return await _db.Statuses.ToListAsync();
        }

        [Route("Create")]
        [HttpPost]
        public async Task<ActionResult<StatusModel>> Create(StatusModel Status)
        {
            if(ModelState.IsValid)
            {
                if (Status.StatusName != null)
                {
                    var statuses = _db.Statuses.Any();
                    if(statuses)
                    {
                        foreach(var status in _db.Statuses)
                        {
                            if (Status.StatusName == status.StatusName)
                            {
                                ModelState.AddModelError("StatusName", "Жанр уже существует");
                                return BadRequest(ModelState);
                            }
                            else
                            {
                                _db.Statuses.Add(Status);
                                await _db.SaveChangesAsync();
                                return StatusCode(201);
                            }
                        }
                    }
                    else
                    {
                        _db.Statuses.Add(Status);
                        await _db.SaveChangesAsync();
                        return StatusCode(201);
                    }
                }
                return BadRequest("StatusName null");
            }
            return BadRequest("ModelState.Invalid");
           
        }

    }
}
