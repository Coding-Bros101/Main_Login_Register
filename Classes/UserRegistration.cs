namespace MainLogin.Classes
{
    public class UserRegistration
    {
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int UserRoleRestriction { get; set; }
        public int UserPlatformRestriction { get; set; }

    }
}
