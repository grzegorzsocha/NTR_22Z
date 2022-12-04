using LibraryManager.Models.Entities;

namespace LibraryManager.Models.Books
{
    public class IndexViewModel
    {
        public string SearchString { get; set; }
        public List<Book> Books { get; set; }
        public int BookId { get; set; }
    }
}
