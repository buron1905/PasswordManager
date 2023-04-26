using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Settings : Entity
    {
        [Required]
        public bool SavePassword { get; set; } = false;


        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

    }
}
