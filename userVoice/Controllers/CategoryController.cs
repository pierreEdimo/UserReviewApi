using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using userVoice.DBContext;
using userVoice.DTo;
using userVoice.Model;
using userVoice.Queryclasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace userVoice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly DatabaseContext _context; 

        public CategoryController(DatabaseContext context)
        {
            _context = context;

            _context.Database.EnsureCreated();
        }
        // GET: api/<CategoryController>
        [HttpGet(Name = nameof(GetAllCategories))]
        public async  Task<ActionResult<IEnumerable<CategoryDTo>>> GetAllCategories([FromQuery] UserQueryParameter queryParameter)
        {
            IQueryable<Category> categories = _context.categories;

            if (!string.IsNullOrEmpty(queryParameter.sortBy))
            {
                if (typeof(Category).GetProperty(queryParameter.sortBy) != null)
                {
                    categories = categories.OrderByCustom(queryParameter.sortBy, queryParameter.SortOrder);
                }
            }


            return await categories.Include(a => a.Items).ThenInclude(a => a.Reviews).Select(x => GetCategoryToDTo(x)).ToListAsync(); 
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTo>> GetCategory(int id)
        {
            IQueryable<Category> categories = _context.categories; 

            var category = await categories.Include(a => a.Items).ThenInclude(a => a.Reviews).FirstOrDefaultAsync(x => x.Id == id); 

            if(category == null)
            {
                return NotFound(); 
            }

            return GetCategoryToDTo(category);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public async Task<ActionResult<CategoryDTo>> Post(CategoryDTo categoryDTo)
        {
            var category = new Category
            {
                Name = categoryDTo.Name,
                ImageUrl = categoryDTo.ImageUrl
            };

            _context.categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, CategoryToDTo(category)); 
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id,  CategoryDTo categoryDTo)
        {
            if(id != categoryDTo.Id)
            {
                return BadRequest(); 
            }

            var category = await _context.categories.FindAsync(id); 


            if(category == null)
            {
                return NotFound(); 
            }

            category.Name = categoryDTo.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var category = await _context.categories.FindAsync(id); 
            if(category == null)
            {
                return NotFound(); 
            }

            _context.categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        private bool CategoryExists(int id)
        {
            return _context.categories.Any(e => e.Id == id); 
        }

        private static CategoryDTo CategoryToDTo(Category category) => new CategoryDTo
        {
            Id = category.Id,
            Name = category.Name,
            Items = category.Items, 
            ImageUrl = category.ImageUrl, 
            EntryDate = category.EntryDate
        }; 

        private static CategoryDTo GetCategoryToDTo(Category category) => new CategoryDTo{
             Id = category.Id,
            Name = category.Name,
            Items = category.Items, 
            ImageUrl = category.ImageUrl, 
            numberOfItems = category.Items.Count(), 
            EntryDate = category.EntryDate

        }; 
    }
}
