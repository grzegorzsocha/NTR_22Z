using System.ComponentModel.DataAnnotations;

namespace LibraryManager.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}
