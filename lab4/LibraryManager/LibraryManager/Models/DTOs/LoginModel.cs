using System.ComponentModel.DataAnnotations;

namespace LibraryManager.Models.DTOs
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberLogin { get; set; }

    }
}
