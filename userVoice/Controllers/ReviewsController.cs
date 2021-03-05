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
using Microsoft.AspNetCore.Authorization;

namespace userVoice.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper; 

        public ReviewsController(DatabaseContext context, 
                                 IMapper mapper  )
        {
            _context = context;
            _mapper = mapper; 
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<List<ReviewDTO>>> GetReviews()
        {
          var reviews = await _context.Reviews
                                              .Include(x => x.Author)
                                              .ToListAsync();

            return _mapper.Map<List<ReviewDTO>>(reviews); 
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDTO>> GetReview(string Id)
        {
            var review = await _context.Reviews
                                               .Include(x => x.Author)
                                               .FirstOrDefaultAsync( x => x.AuthorId == Id );

            if (review == null)
            {
                return NotFound();
            }

            var reviewDTO = _mapper.Map<ReviewDTO>(review); 

            return reviewDTO;
        }

        [HttpGet("Filter")]
        public async Task<ActionResult<List<ReviewDTO>>> FilterReviews([FromQuery] FilterReviewDTO filter)
        {
            var queryable = _context.Reviews.AsQueryable();

            if (!String.IsNullOrWhiteSpace(filter.AuthorId))
            {
                queryable = queryable.Where(x => x.AuthorId.Contains(filter.AuthorId)); 
            }

            if(filter.ItemId != 0)
            {
                queryable = queryable.Where(x => x.ItemId == filter.ItemId ); 
            }

            if (!String.IsNullOrWhiteSpace(filter.sortBy))
            {
                if(typeof(Review).GetProperty(filter.sortBy) != null)
                {
                    queryable = queryable.OrderByCustom(filter.sortBy, filter.SortOrder); 
                }
            }

            var items = await queryable.Include(x => x.Author)
                                       .ToListAsync();

            return _mapper.Map<List<ReviewDTO>>(items); 
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(string Id, AddReviewDTO addReview)
        {
            var reviewDB = await _context.Reviews.FirstOrDefaultAsync(x => x.AuthorId == Id); 

            if(reviewDB == null)
            {
                return NotFound(); 
            }

            reviewDB = _mapper.Map(addReview, reviewDB);

            await _context.SaveChangesAsync(); 

            return NoContent();
        }

        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostReview(AddReviewDTO addReview)
        {
            var review = _mapper.Map<Review>(addReview);

            _context.Add(review); 
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ReviewExists(review.AuthorId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            var reviewDTO = _mapper.Map<ReviewDTO>(review); 

            return CreatedAtAction("GetReview", new { id = reviewDTO.AuthorId }, reviewDTO);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(string Id)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(x => x.AuthorId == Id); 

            if( review == null)
            {
                return NotFound(); 
            }

            _context.Reviews.Remove(review); 

            await _context.SaveChangesAsync(); 

            return NoContent();
        }

        private bool ReviewExists(string id)
        {
            return _context.Reviews.Any(e => e.AuthorId == id);
        }
    }
}
