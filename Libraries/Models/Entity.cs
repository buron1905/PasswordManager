using SQLite;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public abstract class Entity
    {
        [PrimaryKey, AutoIncrement]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [SQLite.NotNull]
        public DateTime UDT { get; set; }

        [Required]
        [SQLite.NotNull]
        public DateTime IDT { get; set; }

        [Required]
        [SQLite.NotNull]
        public DateTime DDT { get; set; }

        [Required]
        [SQLite.NotNull]
        public bool Deleted { get; set; }

        [Required]
        [SQLite.NotNull]
        public DateTime UDTLocal { get; set; }
    }
}
