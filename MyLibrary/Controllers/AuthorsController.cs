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
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public AuthorsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // get edited
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Author.ToListAsync());
        }

        //get edited

        // GET: Authors
        [Authorize]
        public async Task<IActionResult> IndexNew()
        {
            return View(await _context.Author.ToListAsync());
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .FirstOrDefaultAsync(m => m.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,FirstName,LastName")] Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        /* public async Task<IActionResult> Edit(int? id)
         {
             if (id == null)
             {
                 return NotFound();
             }

             var author = await _context.Author.FindAsync(id);
             if (author == null)
             {
                 return NotFound();
             }
             return View(author);
         }*/


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .Include(a => a.Books)
                .FirstOrDefaultAsync(ae => ae.AuthorId == id);

            var viewModel = new AuthorEditViewModel
            {
                Author = author,
                CanEdit = author.Books.Count == 0
            };

            if (author == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }



        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AuthorEditViewModel viewModel)
        {
            if (id != viewModel.Author.AuthorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewModel.Author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(viewModel.Author.AuthorId))
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
            return View(viewModel.Author);
        }



        /*[HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AuthorEditViewModel viewModel)
        {

            *//*var author = await _context.Author
                                       .Include(a => a.Books)
                                       .FirstOrDefaultAsync(a => a.AuthorId == id);*//*
            var author = viewModel.Author;

            if (id != author.AuthorId)
            {
                return NotFound();
            }
            if (author == null)
            {
                return NotFound();
            }
            if (author.Books.Count > 0)
            {
                return Forbid();
            }
            else
            {

                ModelState.Remove("FirstName");
                ModelState.Remove("LastName");

                if (ModelState.IsValid)
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                    *//*try
                    {
                        _context.Update(author);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (id != author.AuthorId)
                        {
                            return NotFound();
                        }
                        if (!AuthorExists(author.AuthorId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }*/
                      /*return RedirectToAction(nameof(Index));*//*
                  }
                  return View(viewModel);
              }
          }*/




        /*// GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .FirstOrDefaultAsync(m => m.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Author.FindAsync(id);
            _context.Author.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        /*get Delete*/
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .Include(a => a.Books)
                .FirstOrDefaultAsync(m => m.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }

            var viewModel = new AuthorDeleteViewModel
            {
                Author = author,
                CanDelete = author.Books.Count == 0
            };

            return View(viewModel);
        }

        /*post delete*/

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var author = await _context.Author
                                       .Include(a => a.Books)
                                       .FirstOrDefaultAsync(a => a.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }
            if (author.Books.Count > 0)
            {
                return Forbid();
            }

            _context.Author.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return _context.Author.Any(e => e.AuthorId == id);
        }
    }
}
