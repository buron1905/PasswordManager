using SQLite;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public abstract class Entity
    {
        [PrimaryKey]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime UDT { get; set; }

        [Required]
        public DateTime IDT { get; set; }

        public DateTime DDT { get; set; }

        public bool Deleted { get; set; }

        [Required]
        public DateTime UDTLocal { get; set; }
    }
}
