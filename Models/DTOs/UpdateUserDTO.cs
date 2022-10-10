using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class UpdateUserDTO
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }

    }
}
