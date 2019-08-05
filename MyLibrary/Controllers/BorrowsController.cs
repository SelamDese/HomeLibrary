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
using MyLibrary.Models.BorrowViewModel;

namespace MyLibrary.Controllers
{
    public class BorrowsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        //Getting the current user in the system (whoever is logged in)
        public Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public BorrowsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Borrows
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var userId = user.Id;
            return View(await _context.Borrow.Where(br => br.UserId == userId).ToListAsync());
        }
        /*Get borrow*/
        public async Task<IActionResult> borrowCart()
        {
            var user = await GetCurrentUserAsync();
            /*var viewModel = new BorrowIndexViewModel();*/
            /*return View(viewModel);*/

            return View(await _context.Borrow.Where(b => b.UserId == user.Id).ToListAsync());

        }
        /*public async Task<IActionResult> borrowCart()
        {
            var user = await GetCurrentUserAsync();

            var book = await _context.Book.FirstOrDefaultAsync(b => b.UserId == user.Id);
            *//*.Include(b => b.Title)
            .Include(b => b.UserId)
            .FirstOrDefaultAsync(b => b.UserId != user.Id);*//*


            var borrow = await _context.Borrow.FirstOrDefaultAsync(br => br.DateReturned == null);
                *//*.Include(br => br.DateBorrowed)
                .FirstOrDefaultAsync(br => br.DateReturned == null);*//*

            BorrowCartViewModel viewModel = new BorrowCartViewModel();

            *//*return View(await _context.Borrow.Where(b => b.UserId == user.Id).ToListAsync());*//*
            return View(viewModel);

        }*/

        // GET: Borrows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrow
                .Include(b => b.User)
                .Include(b => b.books)
                .FirstOrDefaultAsync(m => m.BorrowId == id);
            if (borrow == null)
            {
                return NotFound();
            }

            return View(borrow);
        }

        // GET: Borrows/Create
        /*public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            ViewData["BookId"] = new SelectList(_context.Book, "BookId", "Title");
            return View();
        }*/

        // POST: Borrows/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( int id)
        {
            var user = await GetCurrentUserAsync();
            try
            {
                Book bookToBorrow = await _context.Book.SingleOrDefaultAsync(b => b.BookId == id && b.UserId != user.Id);

                ModelState.Remove("UserId");
                ModelState.Remove("BookId");
                ModelState.Remove("DateBorrowed");

                if (ModelState.IsValid)
                {
                    var borrow = new Borrow();
                    borrow.UserId = user.Id;
                    borrow.BookId = bookToBorrow.BookId;
                    borrow.DateBorrowed = DateTime.Today;
                    borrow.DateReturned = null;
                    _context.Add(borrow);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(borrowCart));
                /*return RedirectToAction("Index", "Borrows");*/
            }
            catch(Exception ex)
            {
                return RedirectToAction(nameof(borrowCart));
            }
        }


        /*  delete when returned */
        // POST: Borrows/Delete/5
        [Authorize]
        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Borrow returnABook = await _context.Borrow.FirstOrDefaultAsync(br => br.BookId == id);
            ModelState.Remove("BorrowId");
            _context.Borrow.Remove(returnABook);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(borrowCart));
        }

        // GET: Borrows/Edit/5
        /*public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrow.FindAsync(id);
            if (borrow == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", borrow.UserId);
            ViewData["BookId"] = new SelectList(_context.Book, "BookId", "Title", borrow.BookId);
            return View(borrow);
        }*/

        // POST: Borrows/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BorrowId,DateBorrowed,DateReturned,BookId,UserId")] Borrow borrow)
        {
            if (id != borrow.BorrowId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowExists(borrow.BorrowId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", borrow.UserId);
            ViewData["BookId"] = new SelectList(_context.Book, "BookId", "Title", borrow.BookId);
            return View(borrow);
        }*/

        // GET: Borrows/Delete/5
        /*public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrow
                .Include(b => b.User)
                .Include(b => b.books)
                .FirstOrDefaultAsync(m => m.BorrowId == id);
            if (borrow == null)
            {
                return NotFound();
            }

            return View(borrow);
        }*/

     /*   // POST: Borrows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var borrow = await _context.Borrow.FindAsync(id);
            _context.Borrow.Remove(borrow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool BorrowExists(int id)
        {
            return _context.Borrow.Any(e => e.BorrowId == id);
        }
    }
}
