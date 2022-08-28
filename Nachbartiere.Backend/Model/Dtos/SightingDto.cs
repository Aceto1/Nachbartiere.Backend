using Nachbartiere.Backend.Database.Entities;

namespace Nachbartiere.Backend.Model.Dtos
{
    public class SightingDto : Sighting
    {
        public SightingDto(Sighting sighting, string base64Picture)
        {
            Location = sighting.Location;
            CreatedBy = sighting.CreatedBy;
            CreatedAt = sighting.CreatedAt;
            Count = sighting.Count;
            Id = sighting.Id;
            Description = sighting.Description;
            AnimalKind = sighting.AnimalKind;
            Picture = base64Picture;
        }

        public string Picture { get; set; }
    }
}
