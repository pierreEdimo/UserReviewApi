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
    public class ReviewsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private IQueryable<Review> reviews; 

        public ReviewsController(DatabaseContext context)
        {
            _context = context;

            _context.Database.EnsureCreated();

           reviews = _context.reviews;
        }

        // GET: api/Reviews
        [HttpGet(Name = nameof(Getreviews)) ]
        public async Task<ActionResult<IEnumerable<ReviewDTo>>> Getreviews([FromQuery] UserQueryParameter queryParameter)
        {
            

            if (!string.IsNullOrEmpty(queryParameter.sortBy))
            {
                if (typeof(Review).GetProperty(queryParameter.sortBy) != null)
                {
                    reviews = reviews.OrderByCustom(queryParameter.sortBy, queryParameter.SortOrder);
                }
            }

            if (!string.IsNullOrEmpty(queryParameter.itemId.ToString()))
            {
                reviews = reviews.Where(p => p.ItemId.ToString().Contains(queryParameter.itemId.ToString()));
            }


            if (!string.IsNullOrEmpty(queryParameter.authorId.ToLower()))
            {
                reviews = reviews.Where(p => p.AuthorId.ToLower().Contains(queryParameter.authorId.ToLower()));
            }

            return await reviews.Include(a => a.Author)
                                 .ThenInclude(a => a.getReviews)
                                .Include(a => a.Item)         
                                .Select(x => reviewToDTo(x)).ToListAsync();

        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDTo>> GetReview(String id)
        {
           

            var review = await reviews.Include(a => a.Author)
                                        .ThenInclude(a => a.getReviews)
                                      .Include(a => a.Item)
                                      .FirstOrDefaultAsync(x => x.AuthorId == id); 

            if (review == null)
            {
                return NotFound();
            }

            return reviewToDTo(review);
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(String id, ReviewDTo reviewDTo)
        {
         

            if (id != reviewDTo.AuthorId)
            {
                return BadRequest();
            }

            var review = await reviews.FirstOrDefaultAsync(x => x.AuthorId == id) ; 

            if(review == null)
            {
                return NotFound(); 
            }

            review.Body = reviewDTo.Body;
            review.ReviewNote = reviewDTo.ReviewNote; 


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReviewDTo>> PostReview(ReviewDTo reviewDTO)
        {
            var review = new Review
            {
                Body = reviewDTO.Body,
                AuthorId = reviewDTO.AuthorId,
                ItemId = reviewDTO.ItemId,
                ReviewNote = reviewDTO.ReviewNote
            }; 

            _context.reviews.Add(review);
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

            return CreatedAtAction( nameof(GetReview) , new { id = review.AuthorId }, createReviewDTO(review) );
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(String id)
        {
            var review = await reviews.FirstOrDefaultAsync(x => x.AuthorId == id);
            if (review == null)
            {
                return NotFound();
            }

            _context.reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(String id)
        {
            return _context.reviews.Any(e => e.AuthorId == id);
        }

        private static ReviewDTo createReviewDTO(Review review) => new ReviewDTo
        {
            Body = review.Body,
            AuthorId = review.AuthorId,
            ItemId = review.ItemId,
            ReviewNote = review.ReviewNote
        };

        private static ReviewDTo reviewToDTo(Review review) => new ReviewDTo
        {
            AuthorId = review.AuthorId, 
            Author = review.Author, 
            Body = review.Body, 
            Item = review.Item, 
            EntryDate = review.EntryDate, 
            ReviewNote = review.ReviewNote, 
            ItemId = review.ItemId
        }; 
    }
}
