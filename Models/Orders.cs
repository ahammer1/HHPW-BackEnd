namespace HHPW_BackEnd.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PaymentID { get; set; }
        public DateTime PublicationDate { get; set; }
        public ICollection<Product> Products { get; set; }
        public Orders() { 
        this.PublicationDate = DateTime.Now;

        }
    }
}
