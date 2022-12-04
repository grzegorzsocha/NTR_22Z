﻿namespace LibraryManager.Models.Entities
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public bool IsCorrectPassword(string password)
        {
            return Password == password;
        }
    }
}
