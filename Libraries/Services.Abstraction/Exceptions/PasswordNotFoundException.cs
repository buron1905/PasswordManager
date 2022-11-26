using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Exceptions
{
    public sealed class PasswordNotFoundException : NotFoundException
    {
        public PasswordNotFoundException(Guid passwordId)
            : base($"The password with the identifier {passwordId} was not found.")
        {
        }

        public PasswordNotFoundException()
            : base($"Password not found.")
        {
        }
    }
}
