using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Exceptions
{
    public sealed class PasswordDoesNotBelongToUserException : BadRequestException
    {
        public PasswordDoesNotBelongToUserException(Guid userId, Guid passwordId)
            : base($"The password with the identifier {passwordId} does not belong to the user with the identifier {userId}")
        {
        }
    }
}
