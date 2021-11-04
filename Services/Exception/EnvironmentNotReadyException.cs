using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.Services.Exception
{
    [Serializable]
    public class EnvironmentNotReadyException : System.Exception
    {
        public string EnVarName { get; }
        public EnvironmentNotReadyException()
        {
        }

        public EnvironmentNotReadyException(string message)
            : base(message)
        {
        }

        public EnvironmentNotReadyException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
        public EnvironmentNotReadyException(string message, string EnVarName)
        : this(message)
        {
            this.EnVarName = EnVarName;
        }
    }
}
