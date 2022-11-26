using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class SettingsDTO
    {
        public Guid Id { get; set; }
        
        [Required]
        public bool SavePassword { get; set; } = false;
    }
}
