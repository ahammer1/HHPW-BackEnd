namespace HHPW_BackEnd.Models
{
    public class OrderProducts
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        
        public int OrderId { get; set; }

        public Product Product { get; set; }

    }
}
