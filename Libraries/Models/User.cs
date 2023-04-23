﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Models.Validation.ModelsValidationProperties;

namespace Models
{
    [Table("AppUsers")]
    public class User : Entity
    {

        [Required]
        [EmailAddress]
        [MaxLength(MaxEmailLength, ErrorMessage = MaxEmailLengthErrorMessage)]
        [SQLite.Unique]
        public string? EmailAddress { get; set; }

        [Required]
        public string? PasswordHASH { get; set; }

        public bool EmailConfirmed { get; set; }

        public string? EmailConfirmationToken { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public string? TwoFactorSecret { get; set; }

    }
}
