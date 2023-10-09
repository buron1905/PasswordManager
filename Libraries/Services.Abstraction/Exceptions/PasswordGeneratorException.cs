using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Exceptions
{
    public class PasswordGeneratorException : Exception
    {
        public PasswordGeneratorException(string message)
            : base(message)
        {
        }
    }
}
