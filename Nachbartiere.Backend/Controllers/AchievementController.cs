using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nachbartiere.Backend.Database;
using Nachbartiere.Backend.Database.Entities;

namespace Nachbartiere.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AchievementController : ControllerBase
    {
        [HttpGet("unlocked/{userId}")]
        public ActionResult<ICollection<Achievement>> GetUnlockedAchievements(string userId)
        {
            using var ctx = new DatabaseContext();

            var unlockedAchievements = ctx.Achievements.Where(m => m.UserId == userId).ToList();

            return Ok(unlockedAchievements);
        }

        [HttpGet("recent/{userId}")]
        public ActionResult<ICollection<Achievement>> GetRecentlyUnlockedAchievements(string userId)
        {
            using var ctx = new DatabaseContext();

            var unlockedAchievements = ctx.Achievements.Where(m => m.UserId == userId).OrderByDescending(m => m.UnlockedAt).Take(5).ToList();

            return Ok(unlockedAchievements);
        }
    }
}
