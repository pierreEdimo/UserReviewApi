using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using userVoice.DBContext;
using userVoice.Model;
using AutoMapper;
using userVoice.DTo;
using System.IO;
using userVoice.Services;
using Microsoft.AspNetCore.Authorization;

namespace userVoice.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _storageService;
        private String containerName = "Items"; 

        public ItemsController(DatabaseContext context,
                               IFileStorageService storageService,
                                IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
            _storageService = storageService; 

        }

        [AllowAnonymous]
        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<List<ItemDTO>>> Getitems()
        {
            IQueryable<Item> _items = _context.Items; 
          
            var items = await _items.Include(x => x.Genre)
                                            .Include(x => x.Reviews)
                                               .ThenInclude(x => x.Author )
                                            .Select(x => new Item() { 
                                                Id = x.Id,
                                                Name = x.Name, 
                                                Picture = x.Picture,
                                                OpeningDate = x.OpeningDate,
                                                Reviews = x.Reviews,
                                                Genre = x.Genre,
                                                Description = x.Description,
                                                GenreId = x.GenreId, 
                                                Rating = x.Reviews.Count() != 0 ?(double)( x.Reviews.Sum(x => x.Rate) / x.Reviews.Count()) : 0.0 
                                            }).ToListAsync();

           
            return _mapper.Map<List<ItemDTO>>(items); 
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<ItemDTO>>> Filter([FromQuery] FilterItemDTO filter)
        {
            var queryable = _context.Items.AsQueryable();

            if (!String.IsNullOrWhiteSpace(filter.Name))
            {
                queryable = queryable.Where(x => x.Name.Contains(filter.Name)); 
            }

            if (!String.IsNullOrWhiteSpace(filter.sortBy))
            {
                if (typeof(Item).GetProperty(filter.sortBy) != null)
                {
                    queryable = queryable.OrderByCustom(filter.sortBy, filter.SortOrder);
                }
            }

            if(filter.GenreId != 0)
            {
                queryable = queryable.Where(x => x.GenreId == filter.GenreId); 
            }

            queryable = queryable.Take(filter.Size); 


            var items = await queryable.Include(x => x.Genre)
                                            .Include(x => x.Reviews)
                                               .ThenInclude(x => x.Author)
                                            .Select(x => new Item()
                                            {
                                                Id = x.Id,
                                                Name = x.Name,
                                                Picture = x.Picture,
                                                OpeningDate = x.OpeningDate,
                                                Reviews = x.Reviews,
                                                Genre = x.Genre,
                                                Description = x.Description,
                                                GenreId = x.GenreId,
                                                Rating = x.Reviews.Count() != 0 ? (double)(x.Reviews.Sum(x => x.Rate) / x.Reviews.Count()) : 0.0
                                            }).ToListAsync();

            return _mapper.Map<List<ItemDTO>>(items); 
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetItem(int Id)
        {
         
           
            var item = await _context.Items.Include(x => x.Genre)
                                           .Include(x => x.Reviews)
                                              .ThenInclude(x => x.Author)
                                           .FirstOrDefaultAsync( x=> x.Id == Id) ;
           
            if (item == null)
            {
                return NotFound();
            }

            var itemDTO = _mapper.Map<ItemDTO>(item); 

            return itemDTO;
        }

        [Authorize(Roles = "Admin")]
        // PUT: api/Items/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int Id, [FromForm] CreateItemDTO updateItem )
        {
            var itemDB = await _context.Items.FirstOrDefaultAsync(x => x.Id == Id); 

            if(itemDB == null)
            {
                return NotFound(); 
            }

            itemDB = _mapper.Map(updateItem, itemDB);

           

            if( updateItem.Picture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await updateItem.Picture.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(updateItem.Picture.FileName);
                    itemDB.Picture = await _storageService.EditFile(content, extension, containerName, itemDB.Picture, updateItem.Picture.ContentType);  
                } 
            }

            await _context.SaveChangesAsync();


            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        // POST: api/Items
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostItem( [FromForm] CreateItemDTO  createItem)
        {
            var item = _mapper.Map<Item>(createItem); 

            if(createItem.Picture != null)
            {
                using(var memoryStream = new MemoryStream())
                {
                    await createItem.Picture.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(createItem.Picture.FileName);
                    item.Picture = await _storageService.SaveFile(content, extension, containerName, createItem.Picture.ContentType); 
                }
            }

            _context.Add(item);

            await _context.SaveChangesAsync();

            var itemDTO = _mapper.Map<ItemDTO>(item); 

            return CreatedAtAction("GetItem", new { id = itemDTO.Id }, itemDTO);
        }

        [Authorize(Roles = "Admin")]
        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int Id)
        {
            var exists =  _context.Items.AnyAsync(x => x.Id == Id); 

            if (!await exists)
            {
                return NotFound();
            }

            _context.Remove( new Item() { Id = Id } );
            await _context.SaveChangesAsync();

            return NoContent();
        }

     

       
    }
}
