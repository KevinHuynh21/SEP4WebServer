using System;

namespace WebApplication.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public String Password { get; set; }

        public User(int id, string username, String password)
        {
            Id = id;
            Username = username;
            Password = password;
        }
    }
}