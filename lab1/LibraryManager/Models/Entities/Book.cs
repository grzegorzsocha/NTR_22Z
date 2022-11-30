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
        public string Reserved { get; set; }
        public string Leased { get; set; }

        public void CancelReservation()
        {
            Reserved = "";
            User = "";
        }

        public void MakeReservation(string userName)
        {
            User = userName;
            Reserved = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        }

        public void ReturnLease()
        {
            User = "";
            Leased = "";
        }

        public void MakeLease(string userName)
        {
            User = userName;
            Reserved = "";
            Leased = DateTime.Now.AddDays(14).ToString("yyyy-MM-dd");
        }

        public bool IsReserved()
        {
            if (Reserved == "")
            {
                return false;
            }
            return true;
        }

        public bool CanReserve()
        {
            return !this.IsReserved() && !this.IsLeased();
        }

        public bool IsLeased()
        {
            if (Leased == "")
            {
                return false;
            }
            return true;
        }

        public bool CanLease()
        {
            return !this.IsLeased() && this.IsReserved();
        }

    }
}
