using Microsoft.EntityFrameworkCore;
using LibraryModel.Models;

using Publisher = LibraryModel.Models.Publisher;

namespace LibraryModel.Data
{
    public class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new LibraryContext(serviceProvider.GetRequiredService<DbContextOptions<LibraryContext>>()))
            {
                if (context.Books.Any())
                {
                    return; // DB has previously been created //
                }

                context.Customers.AddRange(
                    new Customer { Name = "Popescu Marcela", Address = "Str. Plopilor, nr. 24", BirthDate = DateTime.Parse("1979-09-01") },
                    new Customer { Name = "Mihailescu Cornel", Address = "Str. Bucuresti, nr. 45, ap. 2", BirthDate = DateTime.Parse("1969-07-08") }
                );

                context.SaveChanges();

                context.Authors.AddRange(
                    new Author { FirstName = "Mihail", LastName = "Sadoveanu" },
                    new Author { FirstName = "George", LastName = "Calinescu" },
                    new Author { FirstName = "Mircea", LastName = "Eliade" },
                    new Author { FirstName = "Guillaume", LastName = "Musso" },
                    new Author { FirstName = "Cella", LastName = "Serghi" },
                    new Author { FirstName = "J.D.", LastName = "Salinger" }
                );

                context.SaveChanges();

                context.Books.AddRange(
                    new Book
                    {
                        Title = "Baltagul",
                        Price = Decimal.Parse("22"),
                        AuthorID = context.Authors.Single(author => author.LastName == "Sadoveanu").ID
                    },
                    new Book
                    {
                        Title = "Enigma Otiliei",
                        Price = Decimal.Parse("18"),
                        AuthorID = context.Authors.Single(author => author.LastName == "Calinescu").ID
                    },
                    new Book
                    {
                        Title = "Maytrei",
                        Price = Decimal.Parse("27"),
                        AuthorID = context.Authors.Single(author => author.LastName == "Eliade").ID
                    },
                    new Book
                    {
                        Title = "Fata de hartie",
                        Price = Decimal.Parse("24"),
                        AuthorID = context.Authors.Single(author => author.LastName == "Musso").ID
                    },
                    new Book
                    {
                        Title = "Panza de paianjen",
                        Price = Decimal.Parse("21.3"),
                        AuthorID = context.Authors.Single(author => author.LastName == "Serghi").ID
                    },
                    new Book
                    {
                        Title = "De veghe in lanul de secara",
                        Price = Decimal.Parse("20.56"),
                        AuthorID = context.Authors.Single(author => author.LastName == "Salinger").ID
                    }
                );

                context.SaveChanges();

                var orders = new Order[]
                {
                    new Order
                    {
                        CustomerID = context.Customers.Single(customer => customer.Name == "Popescu Marcela").CustomerID,
                        BookID = context.Books.Single(book => book.Title == "Baltagul").ID,
                        OrderDate = DateTime.Parse("2022-12-25")
                    },
                    new Order
                    {
                        CustomerID = context.Customers.Single(customer => customer.Name == "Popescu Marcela").CustomerID,
                        BookID = context.Books.Single(book => book.Title == "Maytrei").ID,
                        OrderDate=DateTime.Parse("2022-12-25")
                    },
                    new Order
                    {
                        CustomerID = context.Customers.Single(customer => customer.Name == "Mihailescu Cornel").CustomerID,
                        BookID = context.Books.Single(book => book.Title == "Fata de hartie").ID,
                        OrderDate=DateTime.Parse("2022-12-28")
                    },
                    new Order
                    {
                        CustomerID = context.Customers.Single(customer => customer.Name == "Mihailescu Cornel").CustomerID,
                        BookID = context.Books.Single(book => book.Title == "Panza de paianjen").ID,
                        OrderDate=DateTime.Parse("2022-12-28")
                    },
                    new Order
                    {
                        CustomerID = context.Customers.Single(customer => customer.Name == "Mihailescu Cornel").CustomerID,
                        BookID = context.Books.Single(book => book.Title == "Enigma Otiliei").ID,
                        OrderDate=DateTime.Parse("2022-12-28")
                    },
                    new Order
                    {
                        CustomerID = context.Customers.Single(customer => customer.Name == "Mihailescu Cornel").CustomerID,
                        BookID = context.Books.Single(book => book.Title == "De veghe in lanul de secara").ID,
                        OrderDate=DateTime.Parse("2022-12-31")
                    }
                };

                foreach (Order e in orders)
                {
                    context.Orders.Add(e);
                }

                context.SaveChanges();

                var publishers = new Publisher[]
                {
                    new Publisher
                    {
                        PublisherName="Humanitas",
                        Adress="Str. Aviatorilor, nr. 40, Bucuresti"
                    },
                    new Publisher
                    {
                        PublisherName="Nemira",
                        Adress="Str. Plopilor, nr. 35, Ploiesti"
                    },
                    new Publisher
                    {
                        PublisherName="Paralela 45",
                        Adress="Str. Cascadelor, nr.22, Cluj-Napoca"
                    },
                };

                foreach (Publisher p in publishers)
                {
                    context.Publishers.Add(p);
                }

                context.SaveChanges();
                var books = context.Books;

                var publishedbooks = new PublishedBook[]
                {
                    new PublishedBook { BookID = books.Single(c => c.Title == "Maytrei" ).ID,
                        PublisherID = publishers.Single(i => i.PublisherName == "Humanitas").ID },
                    new PublishedBook { BookID = books.Single(c => c.Title == "Enigma Otiliei" ).ID,
                        PublisherID = publishers.Single(i => i.PublisherName == "Humanitas").ID},
                    new PublishedBook { BookID = books.Single(c => c.Title == "Baltagul" ).ID,
                        PublisherID = publishers.Single(i => i.PublisherName == "Nemira").ID},
                    new PublishedBook { BookID = books.Single(c => c.Title == "Fata de hartie" ).ID,
                        PublisherID = publishers.Single(i => i.PublisherName == "Paralela45").ID},
                    new PublishedBook { BookID = books.Single(c => c.Title == "Panza de paianjen" ).ID,
                        PublisherID = publishers.Single(i => i.PublisherName == "Paralela45").ID},
                    new PublishedBook { BookID = books.Single(c => c.Title == "De veghe in lanul de secara" ).ID,
                        PublisherID = publishers.Single(i => i.PublisherName == "Paralela45").ID},
                };

                foreach (PublishedBook pb in publishedbooks)
                {
                    context.PublishedBooks.Add(pb);
                }

                context.SaveChanges();
            }
        }
    }
}
