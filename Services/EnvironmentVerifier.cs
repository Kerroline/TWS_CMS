using MangaCMS.Services.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.Services
{
    public class EnvironmentVerifier : IVerifier
    {
        private string[] _EnVarArr =
        {
            "Test:SyncKey",
            "Test:DB_SERVER",
            "Test:DB_NAME",
            "Test:DB_USERNAME",
            "Test:DB_PASSWORD",
        };

        public void Checking()
        {
            foreach(var name in _EnVarArr)
            {
                if (!RuleExist(name))
                {
                    throw new EnvironmentNotReadyException($"The Environment Variable cannot be found: {name} ");
                }
            }
        }

        public bool RuleExist(string rulename)
        {
            if (Environment.GetEnvironmentVariable(rulename) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
