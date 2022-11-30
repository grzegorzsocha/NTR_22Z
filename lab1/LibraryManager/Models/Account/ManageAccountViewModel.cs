using System.ComponentModel.DataAnnotations;

namespace LibraryManager.Models.Account
{
    public class ManageAccountViewModel
    {
        public bool HasBorrowedBooks { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
