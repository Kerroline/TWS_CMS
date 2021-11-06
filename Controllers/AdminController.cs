using MangaCMS.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        MangaCMSContext _db;
        UserManager<IdentityUser> _userManager;
        public AdminController(UserManager<IdentityUser> manager, MangaCMSContext context)
        {
            _db = context;
            _userManager = manager;
        }

    }
}
