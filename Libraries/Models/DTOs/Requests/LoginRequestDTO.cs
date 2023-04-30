﻿using System.ComponentModel.DataAnnotations;
using static Models.Validation.ModelsValidationProperties;

namespace Models.DTOs
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = RequiredEmailAddressErrorMessage)]
        [EmailAddress(ErrorMessage = InvalidEmailAddressFormatErrorMessage)]
        [MaxLength(MaxEmailLength, ErrorMessage = MaxEmailLengthErrorMessage)]
        public string? EmailAddress { get; set; }

        [Required]
        [MinLength(MinPasswordLength, ErrorMessage = MinPasswordLengthErrorMessage)]
        [MaxLength(MaxPasswordLength, ErrorMessage = MaxPasswordLengthErrorMessage)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{12,}$", ErrorMessage = ComplexPasswordErrorMessage)]
        public string? Password { get; set; }
    }
}
