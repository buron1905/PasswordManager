using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models.Validation.ModelsValidation;

namespace Models.DTOs
{
    public class RegisterRequestDTO : LoginRequestDTO
    {
        [Required]
        [Compare(nameof(Password), ErrorMessage = PasswordsDoNotMatchErrorMessage)]
        public string? PasswordAgain { get; set; }
        
        public string? PasswordHASH { get; set; }
    }
}
