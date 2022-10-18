using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class AuthResponseDTO
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpirationDateTime { get; set; }
    }
}
