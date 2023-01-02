namespace Erhan_Ana_Maria_Lab2.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }

        //navigation property
        public ICollection<Order> Orders { get; set; }
    }
}
