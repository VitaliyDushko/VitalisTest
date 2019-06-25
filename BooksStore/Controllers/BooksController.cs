using BooksStore.Data;
using BookStoreUtilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;


namespace BooksStore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public BooksController(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }

        public async Task<ActionResult> GetBooks()
        {
            try
            {
                var books = await _db.Book.ToListAsync();
                if (books.Count != 0)
                {
                    return StatusCode((int)HttpStatusCode.OK, books);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.NoContent, new ErrorResponse { Errors = new List<string> { "No books in the DB" } });
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { Errors = new List<string> { ex.Message } });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var book = await _db.Book.SingleOrDefaultAsync(b => b.Id == id);
                if (book != null)
                {
                    return StatusCode((int)HttpStatusCode.OK, book);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed, new ErrorResponse { Errors = new List<string> { "No book in the DB with this id" } });
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { Errors = new List<string> { ex.Message } });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Book value)
        {
            try
            {
                var book = await _db.Book.AddAsync(value);
                await _db.SaveChangesAsync();
                return StatusCode((int)HttpStatusCode.OK, book.Entity);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { Errors = new List<string> { ex.Message } });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]Book value)
        {
            try
            {
                var book = await _db.Book.SingleOrDefaultAsync(b => b.Id == id);
                _db.Entry(book).CurrentValues.SetValues(value);
                _db.Entry(book).OriginalValues["RowVersion"] = value.RowVersion;
                _db.Book.Update(book);
                await _db.SaveChangesAsync();
                return StatusCode((int)HttpStatusCode.OK, book);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed, new ErrorResponse { Errors = new List<string> { ex.Message } });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { Errors = new List<string> { ex.Message } });
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var book = await _db.Book.SingleOrDefaultAsync(b => b.Id == id);
                _db.Book.Remove(book);
                await _db.SaveChangesAsync();
                return StatusCode((int)HttpStatusCode.OK, true);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed, new ErrorResponse { Errors = new List<string> { ex.Message } });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { Errors = new List<string> { ex.Message } });
            }
        }
    }
}
