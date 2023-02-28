using System.ComponentModel.DataAnnotations;
using static Models.Validation.ModelsValidation;

namespace Models.DTOs
{
    public class RegisterRequestDTO : LoginRequestDTO
    {
        [Required(ErrorMessage = RequiredConfirmPasswordErrorMessage)]
        [Compare(nameof(Password), ErrorMessage = PasswordsDoNotMatchErrorMessage)]
        public string? ConfirmPassword { get; set; }

        public string? PasswordHASH { get; set; }
    }
}
