using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class AuthResponseDTO
    {
        public string? JweToken { get; set; }
        public DateTime ExpirationDateTime { get; set; }
        
        [JsonIgnore]
        public string? RefreshToken { get; set; }
    }
}
