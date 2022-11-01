using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Exceptions
{
    public sealed class RefreshTokenNotFoundException : NotFoundException
    {
        public RefreshTokenNotFoundException() : base("Refresh token not found.")
        {
        }

        public RefreshTokenNotFoundException(Guid refreshTokenId)
            : base($"The refresh token with the identifier {refreshTokenId} was not found.")
        {
        }
    }
}
