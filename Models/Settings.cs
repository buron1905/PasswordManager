using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Settings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [Required]
        public bool SavePassword { get; set; } = false;


        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User? User { get; set; }

    }
}
