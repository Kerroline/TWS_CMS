using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.Services.Authentication
{
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}
