using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Erhan_Ana_Maria_Lab2.Data;
using Erhan_Ana_Maria_Lab2.Models;

namespace Erhan_Ana_Lab2.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        /* 
         * sortOrder -> param within the query string of the URL
         * At first when called -> default case 
         * When the user clicks on the header of a column -> sortOrder param is sent out in a query string
         *
         * The 2 ViewData elements (TitleSortParm and PriceSortParm) -> used by the view 
         * for configuring the col's headers as hyperlinks with query strings
         */

        // GET: Books
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParam"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PriceSortParam"] = sortOrder == "Price" ? "price_desc" : "Price";
            
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var books = from b in _context.Books select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                case "Price":
                    books = books.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    books = books.OrderByDescending(b => b.Price);
                    break;
                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }

            int pageSize = 2;
            // pageNumber ?? 1 -> returns the page nr value if it has a value or 1 if pageNumber is null
            return View(await PaginatedList<Book>.CreateAsync(books.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            /*var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.ID == id);*/

            // Include & ThenInclude force the context obj to load Books.Orders navigation property & for each Order -> Order.Customer navigation property
            var book = await _context.Books
                .Include(s => s.Orders)
                .ThenInclude(e => e.Customer) 
                .AsNoTracking() // improves performance when the returned entities cannot be updated during the lifetime of the current context
                .FirstOrDefaultAsync(m => m.ID == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            DbSet<Author> authors = _context.Authors;

            var authorsQuery = from author in authors
                               select new { ID = author.ID, FullName = author.FirstName + " " + author.LastName };

            ViewData["AuthorID"] = new SelectList(authorsQuery, "ID", "FullName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create([Bind("Title,Price,AuthorID")] Book book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "ID", book.AuthorID);
            }
            catch(DbUpdateException exc)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again if the problem persists.");
            }

            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            DbSet<Author> authors = _context.Authors;

            var authorsQuery = from author in authors
                               select new { ID = author.ID, FullName = author.FirstName + " " + author.LastName };

            ViewData["AuthorID"] = new SelectList(authorsQuery, "ID", "FullName");
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookToUpdate = await _context.Books.FirstOrDefaultAsync(s => s.ID == id);

            if (await TryUpdateModelAsync<Book>(
                bookToUpdate,
                "",
                s => s.Author, s => s.Title, s => s.Price))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException exc)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again if the problem persists");
                }
            }
            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "ID", bookToUpdate.AuthorID);
            return View(bookToUpdate);
        }

        /* 
         * Similar to edit & create ops => delete implies 2 action methods
         * 1 -> GET method -> displays the view in which the user can abort the operation 
         * If the user approves the deletion => 1 -> POST -> HttpPost Delete is called to perform the actual deletion         * 
         */

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                //.Include(b => b.Author)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (book == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again.";
            }

            return View(book);
        }

        /* 
         * Error at db update => HttpPost Delete calls HttpGet Delete & sends a param to indicate the error  
         * HttpGet Delete redisplays the confirmation page together with the error msg, giving the user the possibility to abort/retry the operation
         */

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'LibraryContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return RedirectToAction(nameof(Index));
            }
            
            try
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException exc)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool BookExists(int id)
        {
          return _context.Books.Any(e => e.ID == id);
        }
    }
}
