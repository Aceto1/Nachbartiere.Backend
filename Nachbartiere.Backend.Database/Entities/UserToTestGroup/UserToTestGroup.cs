namespace Nachbartiere.Backend.Database.Entities
{
    public class UserToTestGroup : BaseEntity
    {
        public string UserId { get; set; }

        public int TestGroup { get; set; }
    }
}
