using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManager.Models.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public int Date { get; set; }
        public string Publisher { get; set; }
        public string? Username { get; set; }
        [ForeignKey("Username")]
        public User User { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Reserved { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Leased { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public void CancelReservation()
        {
            Reserved = null;
            Username = null;
        }

        public void MakeReservation(string userName)
        {
            Username = userName;
            Reserved = DateTime.UtcNow.AddDays(1).Date;
        }

        public void ReturnLease()
        {
            Username = null;
            Leased = null;
        }

        public void MakeLease(string userName)
        {
            Username = userName;
            Reserved = null;
            Leased = DateTime.UtcNow.AddDays(14).Date;
        }

        public bool IsReserved()
        {
            return Reserved.HasValue;
        }

        public bool CanReserve()
        {
            return !this.IsReserved() && !this.IsLeased();
        }

        public bool IsLeased()
        {
            return Leased.HasValue;
        }

        public bool CanLease()
        {
            return !this.IsLeased() && this.IsReserved();
        }

    }
}
