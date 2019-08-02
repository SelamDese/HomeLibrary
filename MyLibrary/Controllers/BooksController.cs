using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Data;
using MyLibrary.Models;
using MyLibrary.Models.BookViewModel;


namespace MyLibrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private object buttonSearch;
        public object AcceptButton { get; private set; }

        //Getting the current user in the system (whoever is logged in)
        public Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public BooksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Books
        
            public async Task<IActionResult> Types()
        {

            var model = new BookCategoriesViewModel();

            var GroupedBooks = await (
                from c in _context.Category
                join b in _context.Book
                on c.CategoryId equals b.CategoryId
                group new { c, b } by new { c.CategoryId, c.CategoryName } into grouped
                select new GroupedBooks
                {
                    TypeId = grouped.Key.CategoryId,
                    TypeName = grouped.Key.CategoryName,
                    BookCount = grouped.Select(x => x.b.BookId).Count(),
                    Books = grouped.Select(x => x.b).Take(3).ToList()
                }).ToListAsync();

            model.GroupedBooks = GroupedBooks;
            return View(model);
        }
        public async Task<IActionResult> Index()
        {
            var books = await _context.Book
                    .Include(b => b.User)
                    .Include(b => b.catagory)
                    .Take(20)
                    .ToListAsync();
            return View(books);

        }

        [Authorize]
        public async Task<IActionResult> MyBookIndex()
        {
            //setting the user obj to this variable
            var user = await GetCurrentUserAsync();
            //return a view
            return View(await _context.Book.Where(b => b.UserId == user.Id).ToListAsync());

        }

        //Adding a search bar

        public async Task<IActionResult> Search(string searchBooks)
        {
            var booksSearch = from b in _context.Book
                                 select b;

            if (!String.IsNullOrEmpty(searchBooks))
            {
                booksSearch = booksSearch.Where(s => s.Title.Contains(searchBooks));
            }

            //Allows for enter keypress
            this.AcceptButton = this.buttonSearch;

            return View(await booksSearch.ToListAsync());
        }
        
        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.User)
                .Include(b => b.catagory)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
       
        [Authorize]
        public IActionResult Create()
        {
            var bookCategoriesComplete = _context.Category;

            var authorListComplete = _context.Author;

            List<SelectListItem> bookCategories = new List<SelectListItem>();

            List<SelectListItem> authorList = new List<SelectListItem>();

            bookCategories.Insert(0, new SelectListItem
            {
                Text = " Book Categories",
                Value = ""
            });

            authorList.Insert(0, new SelectListItem
            {
                Text = " Auther ",
                Value = ""
            });

            foreach (var bC in bookCategoriesComplete)
            {
                SelectListItem li = new SelectListItem
                {
                    Value = bC.CategoryId.ToString(),
                    Text = bC.CategoryName
                };
                bookCategories.Add(li);
            }
            foreach (var al in authorListComplete)
            {
                SelectListItem lii = new SelectListItem
                {
                    Value = al.AuthorId.ToString(),
                    Text = al.FullName
                };
                authorList.Add(lii);
            }

            BookCreateViewModel viewModel = new BookCreateViewModel();

            viewModel.BookTypeList = bookCategories;
            viewModel.AuthorList = authorList;

            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName");
            ViewData["AuthorId"] = new SelectList(_context.Author, "AuthorId", "FullName");
            return View(viewModel);
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(BookCreateViewModel viewModel)
        {
            ModelState.Remove("Book.Title");
            ModelState.Remove("Book.Language");
            ModelState.Remove("Book.AuthorId");
            ModelState.Remove("Book.UserId");
            ModelState.Remove("Book.User");

            ApplicationUser user = await GetCurrentUserAsync();

            viewModel.Book.User = user;
            viewModel.Book.UserId = user.Id;

            if (ModelState.IsValid)
            {

                _context.Add(viewModel.Book);

                await _context.SaveChangesAsync();

            return RedirectToAction("MyBookIndex", new { id = viewModel.Book.BookId.ToString() });
            }
            return View(viewModel.Book);
        }

        // GET: Books/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            var viewModel = new BookEditViewModel
            {
                Book = book,
                CategoryList = await _context.Category.ToListAsync(),
                AvailableAuthors = await _context.Author.ToListAsync()
            };
            return View(viewModel);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, BookEditViewModel viewModel)
        {
            var book = viewModel.Book;
            if (id != book.BookId)
            {
                return NotFound();
            }

            ModelState.Remove("Book.Title");
            ModelState.Remove("Book.Language");
            ModelState.Remove("Book.AuthorId");
            ModelState.Remove("Book.UserId");
            ModelState.Remove("Book.User");

            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = await GetCurrentUserAsync();
                    book.UserId = user.Id;
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyBookIndex));
            }
            viewModel.CategoryList = await _context.Category.ToListAsync();
            viewModel.AvailableAuthors = await _context.Author.ToListAsync();
            return View(viewModel);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.User)
                .Include(b => b.catagory)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.BookId == id);
        }
    }
}
