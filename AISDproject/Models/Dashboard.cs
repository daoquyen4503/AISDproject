namespace AISDproject.Models
{
    public class Dashboard
    {
        public int TotalOrders { get; set; }
        public int TotalReservations { get; set; }
        public decimal RevenueToday { get; set; }
        public List<MenuItem> TopMenuItems { get; set; }
    }

}
