using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Data;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(ProductsAPIDbContext db) : ControllerBase
    {
        private readonly ProductsAPIDbContext _db = db;

        private string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _db.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _db.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            var original = await _db.Products.FindAsync(product.Id);

            if (id != product.Id || original.UserId != UserId)
            {
                return BadRequest();
            }

            _db.Entry(original).CurrentValues.SetValues(product);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            product.UserId = UserId;

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            if(product.UserId != UserId)
            {
                return Unauthorized();
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _db.Products.Any(e => e.Id == id);
        }

    }
}
