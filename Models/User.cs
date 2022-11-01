using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Models.Validation.ModelsValidation;

namespace Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [Required]
        [EmailAddress]
        [MaxLength(MaxEmailLength, ErrorMessage = MaxEmailLengthErrorMessage)]
        public string? Email { get; set; }

        [Required]
        public string? PasswordHASH { get; set; }


        public Settings? Settings { get; set; }
        public ICollection<Password>? Passwords { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }

    }
}
