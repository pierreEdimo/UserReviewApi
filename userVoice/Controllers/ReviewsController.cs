using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using userVoice.DBContext;
using userVoice.Model;
using AutoMapper;
using userVoice.DTo;

namespace userVoice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper; 

        public ReviewsController(DatabaseContext context, 
                                 IMapper mapper )
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

        [HttpGet("[action]")]
        public async Task<ActionResult<List<ReviewDTO>>> Filter([FromQuery] FilterReviewDTO filter)
        {
            var queryable =  _context.Reviews.AsQueryable();

            if (!String.IsNullOrWhiteSpace(filter.AuthorId))
            {
                queryable = queryable.Where(x => x.AuthorId == filter.AuthorId); 
            }

            if(filter.ItemId != 0)
            {
                queryable = queryable.Where(x => x.ItemId == filter.ItemId); 
            }

            if (!String.IsNullOrWhiteSpace(filter.sortBy))
            {
                if(typeof(Review).GetProperty(filter.sortBy) != null)
                {
                    queryable = queryable.OrderByCustom(filter.sortBy, filter.SortOrder); 
                }
            }

            var reviews = await queryable
                .Include(x => x.Author)
                .ToListAsync();

            return _mapper.Map<List<ReviewDTO>>(reviews); 
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDTO>> GetReview(int Id)
        {
            var review = await _context.Reviews
                .Include(x => x.Author)
                .FirstOrDefaultAsync(x => x.Id == Id );

            if (review == null)
            {
                return NotFound();
            }

            return _mapper.Map<ReviewDTO>(review);
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int Id,[FromBody] AddReviewDTO updateReview)
        {
            var reviewDb = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == Id );
            
            if(reviewDb == null)
            {
                return NotFound(); 
            }

            reviewDb = _mapper.Map(updateReview, reviewDb);

            await _context.SaveChangesAsync(); 
            
            return NoContent();
        }

        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostReview([FromBody] AddReviewDTO addreview)
        {
            var review = _mapper.Map<Review>(addreview);

            _context.Add(review); 
            
            await _context.SaveChangesAsync();

            var reviewDTO = _mapper.Map<ReviewDTO>(review); 

            return CreatedAtAction("GetReview", new { id = reviewDTO.Id }, reviewDTO);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int Id)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == Id);

            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    
    }
}
