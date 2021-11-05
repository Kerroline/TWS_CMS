using MangaCMS.Services.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.Services
{
    public class EnvironmentVerifier : IVerifier
    {
        public void Checking()
        {
            foreach (string name in Enum.GetNames(typeof(EnVarEnum)))
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
