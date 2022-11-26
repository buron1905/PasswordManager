using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Exceptions
{
    public sealed class SettingsNotFoundException : NotFoundException
    {
        public SettingsNotFoundException(Guid settingsId)
            : base($"The settings with the identifier {settingsId} was not found.")
        {
        }

        public SettingsNotFoundException()
            : base($"The settings was not found.")
        {
        }
    }
}
