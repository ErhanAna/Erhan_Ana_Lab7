using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryModel.Models
{
    public class City
    {
        public int ID { get; set; }

        [Display(Name = "City name")]
        public string? CityName { get; set; }
        public ICollection<Customer>? Customers { get; set; }
    }
}