using Nachbartiere.Backend.Database;
using Nachbartiere.Backend.Database.Enum;

namespace Nachbartiere.Backend.Manager
{
    public static class AchievementManager
    {
        public static List<Achievement> CheckForNewAchievements(string userId)
        {
            var newAchievements = new List<Achievement>();

            if (!CheckForTestGroup(userId))
                return newAchievements;

            var sightingsCount = 0;
            var animalKindCount = 0;
            var streakLength = 0;
            var animalKindsToday = 0;

            using (var ctx = new DatabaseContext())
            {
                sightingsCount = ctx.Sightings.Where(m => m.CreatedBy == userId).Count();
                animalKindCount = ctx.Sightings.Where(m => m.CreatedBy == userId).GroupBy(m => m.AnimalKind).Count();
                animalKindsToday = ctx.Sightings.Where(m => m.CreatedBy == userId && m.CreatedAt.Date == DateTime.UtcNow.Date).GroupBy(m => m.AnimalKind).Count();

                var currentDate = DateTime.UtcNow.Date;
                streakLength = 0;

                while (ctx.Sightings.Where(m => m.CreatedBy == userId && m.CreatedAt.Date == currentDate).Any())
                {
                    streakLength++;
                    currentDate = currentDate.AddDays(-1);
                }
            }

            if(sightingsCount >= 50 && !CheckForUnlockedAchievement(Achievement.Sightings50, userId))
            {
                AddAchievement(Achievement.Sightings50, userId);
                newAchievements.Add(Achievement.Sightings50);
            }
            else if (sightingsCount >= 35  && !CheckForUnlockedAchievement(Achievement.Sightings35, userId))
            {
                AddAchievement(Achievement.Sightings35, userId);
                newAchievements.Add(Achievement.Sightings35);
            }
            else if (sightingsCount >= 20 && !CheckForUnlockedAchievement(Achievement.Sightings20, userId))
            {
                AddAchievement(Achievement.Sightings20, userId);
                newAchievements.Add(Achievement.Sightings20);
            }
            else if (sightingsCount >= 10 && !CheckForUnlockedAchievement(Achievement.Sightings10, userId))
            {
                AddAchievement(Achievement.Sightings10, userId);
                newAchievements.Add(Achievement.Sightings10);
            }
            else if (sightingsCount >= 3 && !CheckForUnlockedAchievement(Achievement.Sightings3, userId))
            {
                AddAchievement(Achievement.Sightings3, userId);
                newAchievements.Add(Achievement.Sightings3);
            }
            else if(sightingsCount >= 1 && !CheckForUnlockedAchievement(Achievement.Sightings1, userId))
            {
                AddAchievement(Achievement.Sightings1, userId);
                newAchievements.Add(Achievement.Sightings1);
            }

            if(animalKindCount >= 10 && !CheckForUnlockedAchievement(Achievement.AnimalKinds10, userId))
            {
                AddAchievement(Achievement.AnimalKinds10, userId);
                newAchievements.Add(Achievement.AnimalKinds10);
            }
            else if (animalKindCount >= 5 && !CheckForUnlockedAchievement(Achievement.AnimalKinds5, userId))
            {
                AddAchievement(Achievement.AnimalKinds5, userId);
                newAchievements.Add(Achievement.AnimalKinds5);
            }
            else if (animalKindCount >= 2 && !CheckForUnlockedAchievement(Achievement.AnimalKinds2, userId))
            {
                AddAchievement(Achievement.AnimalKinds2, userId);
                newAchievements.Add(Achievement.AnimalKinds2);
            }

            if (streakLength >= 7 && !CheckForUnlockedAchievement(Achievement.Streak7, userId))
            {
                AddAchievement(Achievement.Streak7, userId);
                newAchievements.Add(Achievement.Streak7);
            }
            else if (animalKindCount >= 5 && !CheckForUnlockedAchievement(Achievement.Streak5, userId))
            {
                AddAchievement(Achievement.Streak5, userId);
                newAchievements.Add(Achievement.Streak5);
            }
            else if (animalKindCount >= 3 && !CheckForUnlockedAchievement(Achievement.Streak3, userId))
            {
                AddAchievement(Achievement.Streak3, userId);
                newAchievements.Add(Achievement.Streak3);
            }

            if(animalKindsToday >= 5 && !CheckForUnlockedAchievement(Achievement.Lucky, userId)) {
                AddAchievement(Achievement.Lucky, userId);
                newAchievements.Add(Achievement.Lucky);
            }

            return newAchievements;
        }

        private static bool CheckForUnlockedAchievement(Achievement achievement, string userId)
        {
            using var ctx = new DatabaseContext();

            return ctx.Achievements.Any(m => m.UserId == userId && m.AchievementKind == achievement);
        }

        private static void AddAchievement(Achievement achievement, string userId)
        {
            using var ctx = new DatabaseContext();

            var newAchievement = new Database.Entities.Achievement()
            {
                UserId = userId,
                AchievementKind = achievement,
                UnlockedAt = DateTime.UtcNow
            };

            ctx.Achievements.Add(newAchievement);
            ctx.SaveChanges();
        }

        /// <summary>
        /// Nur Nutzer, die der Testgruppe 1 zugewiesen wurden, bekommen Achievements
        /// </summary>
        /// <param name="userId">Nutzer-ID</param>
        /// <returns>Ob der Nutzer in der Testgruppe ist, in der Achievements verteilt werden</returns>
        public static bool CheckForTestGroup(string userId)
        {
            using var ctx = new DatabaseContext();

            return (ctx.UsersToTestGroups.SingleOrDefault(m => m.UserId == userId)?.TestGroup ?? 0) == 1;
        }
    }
}
