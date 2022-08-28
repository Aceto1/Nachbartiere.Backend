namespace Nachbartiere.Backend.Model
{
    public class UserCreateArgument
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public string Nickname { get; set; }

        public bool AcceptPrivacyNotice { get; set; }
    }
}
