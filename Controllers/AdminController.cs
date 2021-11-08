using MangaCMS.DAL;
using MangaCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly MangaCMSContext _db;
        private readonly UserManager<CustomUser> _userManager;
        public AdminController(UserManager<CustomUser> manager, MangaCMSContext context)
        {
            _db = context;
            _userManager = manager;
        }

    }
}
