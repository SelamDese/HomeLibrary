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
using MyLibrary.Models.WishListViewModel;

namespace MyLibrary.Controllers
{
    public class WishListsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public WishListsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: WishLists
        /*public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.wishList.Include(w => w.User).Include(w => w.book);
            return View(await applicationDbContext.ToListAsync());
        }*/
        [Authorize]

        public async Task<IActionResult> wishListIndex()
        {
            var user = await GetCurrentUserAsync();

            var wishLists = await _context.wishList
                .Include(w => w.book)
                .Where(w => w.UserId == user.Id)
                .ToListAsync();

            return View(wishLists);

            /*return View(await _context.wishList.Where(b => b.UserId == user.Id).ToListAsync());*/

        }

     /*   public async Task<IActionResult> wishListIndex()
        {
            var user = await GetCurrentUserAsync();

            var wishLists = await _context.wishList.Where(b => b.UserId == user.Id).ToListAsync();

            WishListIndexViewModel WishListndexviewModel = new WishListIndexViewModel();

            WishListndexviewModel.WishList = wishLists;

            return View(WishListndexviewModel);

            *//*return View(await _context.wishList.Where(b => b.UserId == user.Id).ToListAsync());*//*

        }*/

        // GET: WishLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.wishList
                .Include(w => w.User)
                .Include(w => w.book)
                .FirstOrDefaultAsync(m => m.WishListId == id);
            if (wishList == null)
            {
                return NotFound();
            }

            return View(wishList);
        }

        /*// GET: WishLists/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            ViewData["BookId"] = new SelectList(_context.Book, "BookId", "Title");
            return View();
        }

        // POST: WishLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WishListId,BookId,UserId")] WishList wishList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wishList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", wishList.UserId);
            ViewData["BookId"] = new SelectList(_context.Book, "BookId", "Title", wishList.BookId);
            return View(wishList);
        }*/


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id)
        {
            var user = await GetCurrentUserAsync();
            try
            {
                Book bookToWishList = await _context.Book.SingleOrDefaultAsync(b => b.BookId == id && b.UserId != user.Id);

                ModelState.Remove("UserId");
                ModelState.Remove("WBookId");
                ModelState.Remove("book");
                ModelState.Remove("User");

                if (ModelState.IsValid)
                {
                    var wishList = new WishList();
                    wishList.UserId = user.Id;
                    wishList.BookId = bookToWishList.BookId;
                    _context.Add(wishList);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(wishListIndex));
            }
            catch(Exception ex)
            {
                return RedirectToAction(nameof(wishListIndex));
            }
        }

        /*// GET: WishLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.wishList.FindAsync(id);
            if (wishList == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", wishList.UserId);
            ViewData["BookId"] = new SelectList(_context.Book, "BookId", "Title", wishList.BookId);
            return View(wishList);
        }

        // POST: WishLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WishListId,BookId,UserId")] WishList wishList)
        {
            if (id != wishList.WishListId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wishList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WishListExists(wishList.WishListId))
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
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", wishList.UserId);
            ViewData["BookId"] = new SelectList(_context.Book, "BookId", "Title", wishList.BookId);
            return View(wishList);
        }

        // GET: WishLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.wishList
                .Include(w => w.User)
                .Include(w => w.book)
                .FirstOrDefaultAsync(m => m.WishListId == id);
            if (wishList == null)
            {
                return NotFound();
            }

            return View(wishList);
        }

        // POST: WishLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wishList = await _context.wishList.FindAsync(id);
            _context.wishList.Remove(wishList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        [Authorize]
        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            WishList removeABook = await _context.wishList.FirstOrDefaultAsync(br => br.BookId == id);
            ModelState.Remove("WishListId");
            _context.wishList.Remove(removeABook);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(wishListIndex));
        }

        private bool WishListExists(int id)
        {
            return _context.wishList.Any(e => e.WishListId == id);
        }
    }
}
