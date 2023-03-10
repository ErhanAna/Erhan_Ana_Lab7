using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryModel.Models
{
    public class Book
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        
        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }
        public int? AuthorID { get; set; }
        // ensure non-nullable property -> ?
        public Author? Author { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<PublishedBook>? PublishedBooks { get; set; }
    }
}
