using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Exceptions
{
    public sealed class SettingsDoesNotBelongToUserException : BadRequestException
    {
        public SettingsDoesNotBelongToUserException(Guid userId, Guid settingsId)
            : base($"The settings with the identifier {settingsId} does not belong to the user with the identifier {userId}")
        {
        }
    }
}
