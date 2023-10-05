namespace HHPW_BackEnd.Models
{
    public class OrderStatus
    {
        public int Id { get; set; }
        public string Status { get; set; }

        public ICollection<Orders> Orders { get; set; }
    }
}
