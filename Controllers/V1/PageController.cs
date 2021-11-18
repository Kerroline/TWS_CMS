using MangaCMS.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PageController : ControllerBase
    {
        private readonly MangaCMSContext _mangaContext;
        public PageController(MangaCMSContext context)
        {
            _mangaContext = context;
        }


    }
}
