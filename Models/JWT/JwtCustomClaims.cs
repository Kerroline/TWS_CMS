using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.Models.JWT
{
    public class JwtCustomClaims
    {
        public string Login { get; set; }
        public string Role { get; set; }
    }
}
