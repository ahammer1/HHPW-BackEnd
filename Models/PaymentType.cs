namespace HHPW_BackEnd.Models
{
    public class PaymentType
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public ICollection<Orders> Orders { get; set; }
    }
}
