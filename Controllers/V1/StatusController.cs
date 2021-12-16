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
        private readonly MangaCMSContext _mangaContext;
        public StatusController(MangaCMSContext context)
        {
            _mangaContext = context;
        }

        /// <summary>
        /// Get all statuses.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /GetAll
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <response code="200">List of Statuses</response>
        [Route("GetAll")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusModel>>> GetAll()
        {
            return await _mangaContext.Statuses.ToListAsync();
        }

        /// <summary>
        /// Get once status.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Get/1
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Status by id</response>
        /// /// <response code="400">Status not found</response>
        [Route("Get/{id}")]
        [HttpGet]
        public async Task<ActionResult<StatusModel>> Get(int id)
        {
            var current_status = await _mangaContext.Statuses.FindAsync(id);
            if (current_status != null)
            {
                return current_status;
            }
            return StatusCode(400, "Status not found");
        }

        /// <summary>
        /// Creates a new Status.
        /// </summary>
        /// <param name="status"></param>
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
        /// <response code="400">If the status is exist or model invalid</response>
        [Route("Create")]
        [HttpPost]
        public async Task<ActionResult<StatusModel>> Create(StatusModel status)
        {
            if (ModelState.IsValid)
            {
                foreach (var statusFromDB in _mangaContext.Statuses)
                {
                    if (status.StatusName == statusFromDB.StatusName)
                    {
                        return StatusCode(400, "Status exist");
                    }
                }
                _mangaContext.Statuses.Add(status);
                await _mangaContext.SaveChangesAsync();

                return StatusCode(204, "Status created");

            }
            return StatusCode(400, "ModelState.Invalid");

        }

        /// <summary>
        /// Edit a exist status
        /// </summary>
        /// <param name="Status"></param>
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
        /// <response code="204">If the status is created</response>
        /// <response code="400">If the status is exist or model invalid</response>
        [Route("Edit/{id}")]
        [HttpPut]
        public async Task<ActionResult<StatusModel>> Edit(int id)
        {
            return StatusCode(200, "Status update");
        }

        /// <summary>
        /// Delete a exist status
        /// </summary>
        /// <param name="Status"></param>
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
        /// <response code="204">If the status is created</response>
        /// <response code="400">If the status is exist or model invalid</response>
        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<ActionResult<StatusModel>> Delete(int id)
        {
            return StatusCode(204, "Status delete");
        }

    }
}
