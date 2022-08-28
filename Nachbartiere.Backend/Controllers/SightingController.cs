using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nachbartiere.Backend.Database;
using Nachbartiere.Backend.Database.Entities;
using Nachbartiere.Backend.Manager;
using Nachbartiere.Backend.Model;
using Nachbartiere.Backend.Model.Dtos;
using Sentry;

namespace Nachbartiere.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SightingsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IHub sentryHub;

        public SightingsController(IConfiguration configuration, IHub sentryHub)
        {
            this.configuration = configuration;
            this.sentryHub = sentryHub;
        }

        [HttpPost("add")]
        public ActionResult<List<Database.Enum.Achievement>> AddSighting([FromBody] AddSightingArgument arg)
        {
            using var ctx = new DatabaseContext();

            var sighting = new Sighting
            {
                Count = arg.Count,
                AnimalKind = arg.AnimalKind,
                Description = arg.Description,
                CreatedBy = arg.CreatedBy,
                Location = arg.Location,
                CreatedAt = DateTime.UtcNow
            };

            ctx.Sightings.Add(sighting);
            ctx.SaveChanges();

            var path = configuration.GetValue<string>("ImageSavePath");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Remove prefix "data:image/jpeg;base64," (length fo 23 chars) so that the Convert class recognizes it as a base64 string
            var rawBase64String = arg.Picture.Substring(23);
            var bytes = Convert.FromBase64String(rawBase64String);
            System.IO.File.WriteAllBytes(Path.Combine(path, $"{sighting.Id}.jpeg"), bytes);

            return Ok(AchievementManager.CheckForNewAchievements(arg.CreatedBy));
        }
        
        [HttpGet("recent/{userId}")]
        public ActionResult<ICollection<SightingDto>> GetRecentSightings(string userId)
        {
            using var ctx = new DatabaseContext();

            var sightings = ctx.Sightings.Where(m => m.CreatedBy == userId).OrderByDescending(m => m.CreatedAt).Take(5).ToList();
            var result = new List<SightingDto>();

            foreach (var sighting in sightings)
            {
                var path = configuration.GetValue<string>("ImageSavePath");
                var bytes = System.IO.File.ReadAllBytes(Path.Combine(path, $"{sighting.Id}.jpeg"));
                var picture = Convert.ToBase64String(bytes);

                result.Add(new SightingDto(sighting, "data:image/jpeg;base64," + picture));
            }

            return Ok(result);
        }

        [HttpGet("all/{userId}")]
        public ActionResult<ICollection<SightingDto>> GetAllSightings(string userId)
        {
            using var ctx = new DatabaseContext();

            var sightings = ctx.Sightings.Where(m => m.CreatedBy == userId).OrderByDescending(m => m.CreatedAt).ToList();
            var result = new List<SightingDto>();

            foreach (var sighting in sightings)
            {
                var path = configuration.GetValue<string>("ImageSavePath");
                var bytes = System.IO.File.ReadAllBytes(Path.Combine(path, $"{sighting.Id}.jpeg"));
                var picture = Convert.ToBase64String(bytes);

                result.Add(new SightingDto(sighting, "data:image/jpeg;base64," + picture));
            }

            return Ok(result);
        }


        [HttpGet("{sightingsId}")]
        public ActionResult<SightingDto> GetSighting(int sightingsId)
        {
            using var ctx = new DatabaseContext();

            var sighting = ctx.Sightings.SingleOrDefault(m => m.Id == sightingsId);

            if (sighting == null)
                return NotFound();

            var path = configuration.GetValue<string>("ImageSavePath");
            var bytes = System.IO.File.ReadAllBytes(Path.Combine(path, $"{sighting.Id}.jpeg"));
            var picture = Convert.ToBase64String(bytes);

            var result = new SightingDto(sighting, "data:image/jpeg;base64," + picture);

            return Ok(result);
        }

        [HttpGet("totalSightingsCount/{userId}")]
        public ActionResult<int> GetTotalSightingsCount(string userId)
        {
            using var ctx = new DatabaseContext();

            var sightingsCount = ctx.Sightings.Where(m => m.CreatedBy == userId).Count();
            return Ok(sightingsCount);
        }

        [HttpGet("animalKindCount/{userId}")]
        public ActionResult<int> GetAnimalKindCount(string userId)
        {
            using var ctx = new DatabaseContext();

            var animalKindCount = ctx.Sightings.Where(m => m.CreatedBy == userId).GroupBy(m => m.AnimalKind).Count();
            return Ok(animalKindCount);
        }

        [HttpGet("streakLength/{userId}")]
        public ActionResult<int> GetStreakLength(string userId)
        {
            using var ctx = new DatabaseContext();

            var currentDate = DateTime.UtcNow.Date;
            var streakLength = 0;

            while(ctx.Sightings.Where(m => m.CreatedBy == userId && m.CreatedAt.Date == currentDate).Any())
            {
                streakLength++;
                currentDate = currentDate.AddDays(-1);
            }

            return Ok(streakLength);
        }
    }
}
