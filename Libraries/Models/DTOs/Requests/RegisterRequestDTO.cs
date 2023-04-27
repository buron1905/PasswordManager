using System.ComponentModel.DataAnnotations;
using static Models.Validation.ModelsValidationProperties;

namespace Models.DTOs
{
    public class RegisterRequestDTO : LoginRequestDTO
    {

        //[Compare(nameof(Password), ErrorMessage = PasswordsDoNotMatchErrorMessage)] // not used due to RSA encryption, which makes it impossible to compare passwords
        [Required(ErrorMessage = RequiredConfirmPasswordErrorMessage)]
        public string? ConfirmPassword { get; set; }

    }
}
