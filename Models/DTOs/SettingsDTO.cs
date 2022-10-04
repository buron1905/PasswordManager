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
        [Required]
        public bool SavePassword { get; set; } = false;
    }
}
