namespace Models.DTOs
{
    public class EntityDTO
    {

        public Guid Id { get; set; }

        public DateTime UDT { get; set; }

        public DateTime IDT { get; set; }

        public DateTime DDT { get; set; }

        public bool Deleted { get; set; }

        public DateTime UDTLocal { get; set; }

    }
}
