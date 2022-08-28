using Nachbartiere.Backend.Database.Enum;

namespace Nachbartiere.Backend.Model
{
    public class AddSightingArgument
    {
        public string Picture { get; set; }

        public int Count { get; set; }

        public string CreatedBy { get; set; }

        public string Location { get; set; }

        public string? Description { get; set; }

        public AnimalKind AnimalKind { get; set; }
    }
}
