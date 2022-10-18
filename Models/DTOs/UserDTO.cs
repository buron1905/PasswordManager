﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using static Models.Validation.ModelsValidation;

namespace Models.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        
        [Required]
        [EmailAddress]
        [MaxLength(MaxEmailLength, ErrorMessage = MaxEmailLengthErrorMessage)]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        
        [IgnoreDataMember]
        public string? PasswordHASH { get; set; }
    }
}
