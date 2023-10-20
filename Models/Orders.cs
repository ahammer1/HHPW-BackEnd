namespace HHPW_BackEnd.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PaymentID { get; set; }
        public int StatusId { get; set; }

        public int PaymentTypesId { get; set; }

        public string? Tip { get; set; }

        public string? Review { get; set; }
        public DateTime PublicationDate { get; set; }
        public ICollection<OrderStatus> Status { get; set; }
        public ICollection<Product> Products { get; set; }
        public Orders() { 
        this.PublicationDate = DateTime.Now;
        
        }
        public ICollection<PaymentType> PaymentTypes { get; set; }

    }
}
