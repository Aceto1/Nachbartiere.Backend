using Nachbartiere.Backend.Database.Enum;

namespace Nachbartiere.Backend.Database.Entities
{
    public class Sighting : BaseEntity
    {
        public DateTime CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public string? Location { get; set; }

        public string? Description { get; set; }

        public int Count { get; set; }

        public AnimalKind AnimalKind { get; set; }
    }
}
