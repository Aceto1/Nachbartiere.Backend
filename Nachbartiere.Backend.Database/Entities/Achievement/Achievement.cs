namespace Nachbartiere.Backend.Database.Entities
{
    public class Achievement : BaseEntity
    {
        public Enum.Achievement AchievementKind { get; set; }

        public string UserId { get; set; }

        public DateTime UnlockedAt { get; set; }
    }
}
