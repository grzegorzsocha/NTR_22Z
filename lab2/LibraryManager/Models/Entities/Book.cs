namespace LibraryManager.Models.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public int Date { get; set; }
        public string Publisher { get; set; }
        public string User { get; set; }
        public DateTime? Reserved { get; set; }
        public DateTime? Leased { get; set; }

        public void CancelReservation()
        {
            Reserved = null;
            User = "";
        }

        public void MakeReservation(string userName)
        {
            User = userName;
            Reserved = DateTime.Now.AddDays(1).Date;
        }

        public void ReturnLease()
        {
            User = "";
            Leased = null;
        }

        public void MakeLease(string userName)
        {
            User = userName;
            Reserved = null;
            Leased = DateTime.Now.AddDays(14).Date;
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
