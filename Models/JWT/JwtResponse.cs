using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.Models.JWT
{
    public class JwtResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public JwtResponse(string Token, string RefreshToken)
        {
            this.Token = Token;
            this.RefreshToken = RefreshToken;
        }
    }
}
