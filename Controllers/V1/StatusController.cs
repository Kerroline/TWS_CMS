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

        /// <summary>
        /// Creates a new Project.
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Create
        ///     {
        ///        "StatusName": "Name"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">If the status is created</response>
        /// <response code="400">If the status is exist</response>
        [Route("Create")]
        [HttpPost]
        public async Task<ActionResult<StatusModel>> Create(StatusModel Status)
        {
            if(ModelState.IsValid)
            {
                foreach(var status in _db.Statuses)
                {
                    if (Status.StatusName == status.StatusName)
                    {
                        return StatusCode(400, "Status exist");
                    }
                    else
                    {
                        _db.Statuses.Add(Status);
                        await _db.SaveChangesAsync();

                        List<StatusModel> status_list = await _db.Statuses.ToListAsync();
                        var created_status = status_list.Last();

                        if (created_status.StatusName == Status.StatusName)
                        {
                            return StatusCode(204, "Status created");
                        }
                        else
                        {
                            return StatusCode(500, "Status fail to created");
                        }
                    }
                }
            }
            return StatusCode(400,"ModelState.Invalid");
           
        }

    }
}
