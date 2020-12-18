using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using userVoice.DBContext;
using userVoice.DTo;
using userVoice.Model;
using userVoice.Queryclasses;

namespace userVoice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ItemsController(DatabaseContext context)
        {
            _context = context;

            _context.Database.EnsureCreated();
        }

        // GET: api/Items
        [HttpGet(Name = nameof(Getitems))]
        public async Task<ActionResult<IEnumerable<ItemDTo>>> Getitems([FromQuery] UserQueryParameter queryParameter )
        {
            IQueryable<Item> items  = _context.items;

            if (!string.IsNullOrEmpty(queryParameter.sortBy))
            {
                if(typeof(Item).GetProperty(queryParameter.sortBy) != null)
                {
                    items = items.OrderByCustom(queryParameter.sortBy, queryParameter.SortOrder); 
                }
            }

            if (!string.IsNullOrEmpty(queryParameter.Name))
            {
                items = items.Where(p => p.Name.ToLower().Contains(queryParameter.Name.ToLower())); 
            }

            return await items.Include(a => a.Reviews).ThenInclude(a => a.Comments).Include( a => a.Category).Select(x => GetItemToDTo(x)).ToListAsync(); 
        }

        [HttpGet( "[action]", Name = nameof(GetitemFromCategory))]
        public async Task<ActionResult<IEnumerable<ItemDTo>>> GetitemFromCategory([FromQuery] UserQueryParameter queryParameter)
        {
            IQueryable<Item> items = _context.items;

            if (!string.IsNullOrEmpty(queryParameter.sortBy))
            {
                if (typeof(Item).GetProperty(queryParameter.sortBy) != null)
                {
                    items = items.OrderByCustom(queryParameter.sortBy, queryParameter.SortOrder);
                }
            }

            if (!string.IsNullOrEmpty(queryParameter.categoryId.ToString()))
            {
                items = items.Where(p => p.CategoryId.ToString().Contains(queryParameter.categoryId.ToString()));
            }

            return await items.Include(a => a.Reviews).ThenInclude(a => a.Comments).Include(a => a.Category).Select(x => GetItemToDTo(x)).ToListAsync();
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTo>> GetItem(int id)
        {
            IQueryable<Item> items = _context.items;

            var item = await items.Include(a => a.Reviews).ThenInclude(a => a.Comments).Include(a => a.Category).FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return GetItemToDTo(item);
        }

        // PUT: api/Items/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, ItemDTo item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            var itemToUpdate = await _context.items.FindAsync(id); 

            if(itemToUpdate == null)
            {
                return NotFound(); 
            }

            itemToUpdate.Name = item.Name;
            itemToUpdate.Description = item.Description;
            itemToUpdate.ImageUrl = item.ImageUrl; 
 

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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

        // POST: api/Items
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ItemDTo>> PostItem(ItemDTo itemDTo)
        {
            var item = new Item
            {
                Name = itemDTo.Name,
                Description = itemDTo.Description,
                ImageUrl = itemDTo.ImageUrl,
                CategoryId = itemDTo.CategoryId, 
                ReleaseDate = itemDTo.ReleaseDate, 
                Publisher = itemDTo.Publisher, 
                Genre = itemDTo.Genre
            }; 

            _context.items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, ItemToDTo(item));
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var item = await _context.items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(int id)
        {
            return _context.items.Any(e => e.Id == id);
        }

        private static ItemDTo ItemToDTo(Item item) => new ItemDTo 
        
        {
            Id = item.Id, 
            ReleaseDate = item.ReleaseDate, 
            Publisher = item.Publisher, 
            CategoryId = item.CategoryId, 
            Description = item.Description, 
            Name = item.Name, 
            ImageUrl = item.ImageUrl, 
            EntryDate = item.EntryDate, 
            Category = item.Category, 
            Reviews = item.Reviews, 
            Genre = item.Genre
        };

        private static ItemDTo GetItemToDTo(Item item)
        {
            var Note = 0; 
            if(item.Reviews.Count() != 0 ) Note = item.Reviews.Sum(u => u.ReviewNote ) / item.Reviews.Count(); 
         

            return new ItemDTo

            {
                Id = item.Id,
                ReleaseDate = item.ReleaseDate,
                Publisher = item.Publisher,
                CategoryId = item.CategoryId,
                Description = item.Description,
                Name = item.Name,
                ImageUrl = item.ImageUrl,
                EntryDate = item.EntryDate,
                Category = item.Category,
                Reviews = item.Reviews,
                Genre = item.Genre,
                numberOfReviews = item.Reviews.Count(),
                Note = Note
            };
        }
    }
}
