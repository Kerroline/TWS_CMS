using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.Services
{
    interface IVerifier
    {
        bool RuleExist(string rulename);
        void Checking();
    }
}
