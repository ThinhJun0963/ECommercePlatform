namespace EShop.BLL.DTOs
{
    public class DashboardStatsDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; } 
        public int TotalProductsSold { get; set; }   
        public int TotalCustomers { get; set; }        
    }
}