using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LibraryModel.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }

        [Display(Name = "Date of birth")]
        public DateTime BirthDate { get; set; }

        //navigation property
        public ICollection<Order>? Orders { get; set; }

        public int CityID { get; set; }
        public City? City { get; set; }
    }
}
