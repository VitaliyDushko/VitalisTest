using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookStoreUtilities;

namespace BookStoreMVCClient.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly BookApiService _bookApiService;

        public BooksController(BookApiService bookApiService)
        {
            _bookApiService = bookApiService;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var response = await _bookApiService.GetBooks();
            if (response.Content == null)
            {
                return View(new List<Book>());
            }
            return View(response.Content);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _bookApiService.GetBookById(id.Value);
            if (response == null || response.Content == null)
            {
                return NotFound();
            }

            return View(response.Content);
        }

        //// GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        //// POST: Books/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author")] Book book)
        {
            if (ModelState.IsValid)
            {
                var response = await _bookApiService.CreateBook(book);
                if (!response.Content.HasValue)
                {
                    return View(book);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        //// GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _bookApiService.GetBookById(id.Value);
            if (response == null || response.Content == null)
            {
                return NotFound();
            }
            return View(response.Content);
        }

        //// POST: Books/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,RowVersion")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var response = await _bookApiService.EditBook(book);
                if (response.Content == null)
                {
                    ModelState.AddModelError("Concurrency", "Someone else has already deleted or changed the book record while you were editing it");
                    return View(book);
                }
                return RedirectToAction(nameof(Index));
            }            
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _bookApiService.GetBookById(id.Value);
            if (response == null || response.Content == null)
            {
                return NotFound();
            }

            return View(response.Content);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _bookApiService.DeleteBookById(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
