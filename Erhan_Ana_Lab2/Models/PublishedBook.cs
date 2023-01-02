using Erhan_Ana_Maria_Lab2.Models;

namespace Erhan_Ana_Lab2.Models
{
    public class PublishedBook
    {
        public int PublisherID { get; set; }
        public int BookID { get; set; }
        public Publisher? Publisher { get; set; }
        public Book? Book { get; set; }
    }
}
