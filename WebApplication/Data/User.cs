namespace WebApplication.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Password { get; set; }

        public User(int id, string username, int password)
        {
            Id = id;
            Username = username;
            Password = password;
        }
    }
}