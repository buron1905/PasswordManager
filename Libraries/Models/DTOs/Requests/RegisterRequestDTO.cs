using System.ComponentModel.DataAnnotations;
using static Models.Validation.ModelsValidationProperties;

namespace Models.DTOs
{
    public class RegisterRequestDTO : LoginRequestDTO
    {

        [Required(ErrorMessage = RequiredConfirmPasswordErrorMessage)]
        [Compare(nameof(Password), ErrorMessage = PasswordsDoNotMatchErrorMessage)]
        public string? ConfirmPassword { get; set; }

    }
}
