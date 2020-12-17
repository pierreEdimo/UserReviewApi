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
        

        public ReviewsController(DatabaseContext context)
        {
            _context = context;

            

            _context.Database.EnsureCreated();
        }


        // GET: api/Reviews
        [HttpGet(Name = nameof(Getreviews))]
        public async Task<ActionResult<IEnumerable<ReviewDTo>>> Getreviews([FromQuery] UserQueryParameter queryParameter)
        {
            IQueryable<Review> reviews = _context.reviews;

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

            return await reviews.Include(a => a.Item).Include(a => a.Comments).Select(x => GetReviewToDTo(x)).ToListAsync();
        }

        [HttpGet("[action]", Name = nameof(GetReviewFromAuthor))]
        public async Task<ActionResult<IEnumerable<ReviewDTo>>> GetReviewFromAuthor([FromQuery] UserQueryParameter queryParameter)
        {
            IQueryable<Review> reviews = _context.reviews;

            if (!string.IsNullOrEmpty(queryParameter.sortBy))
            {
                if (typeof(Review).GetProperty(queryParameter.sortBy) != null)
                {
                    reviews = reviews.OrderByCustom(queryParameter.sortBy, queryParameter.SortOrder);
                }
            }

            if (!string.IsNullOrEmpty(queryParameter.authorId.ToLower()))
            {
                reviews = reviews.Where(p => p.AuthorId.ToLower().Contains(queryParameter.authorId.ToLower()));
            }

            return await reviews.Include(a => a.Item).Include(a => a.Comments).Select(x => GetReviewToDTo(x)).ToListAsync();
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDTo>> GetReview(int id)
        {
            IQueryable<Review> reviews = _context.reviews;

            var review = await reviews.Include(a => a.Item).Include(a => a.Comments).FirstOrDefaultAsync(x => x.Id == id);

            if (review == null)
            {
                return NotFound();
            }

            return ReviewToDTo(review);
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, ReviewDTo reviewDTo)
        {
            if (id != reviewDTo.Id)
            {
                return BadRequest();
            }

            var review = await _context.reviews.FindAsync(id);

            if (review == null)
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ReviewDTo>> PostReview(ReviewDTo reviewDTo)
        {
            var review = new Review
            {
             
                Body = reviewDTo.Body,
                AuthorId = reviewDTo.AuthorId,
                ItemId = reviewDTo.ItemId, 
                ReviewNote = reviewDTo.ReviewNote
            };

            _context.reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, ReviewToDTo(review));
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Review>> DeleteReview(int id)
        {
            var review = await _context.reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int id)
        {
            return _context.reviews.Any(e => e.Id == id);
        }

        private static ReviewDTo ReviewToDTo(Review review) => new ReviewDTo
        {
            Id = review.Id,
            ItemId = review.ItemId,
            Body = review.Body,
            AuthorId = review.AuthorId,
            Item = review.Item,
            Comments = review.Comments, 
            ReviewNote = review.ReviewNote
        };

        private static ReviewDTo GetReviewToDTo(Review review) =>  new ReviewDTo{
            Id = review.Id,
            ItemId = review.ItemId,
            Body = review.Body,
            AuthorId = review.AuthorId,
            Item = review.Item,
            numberOfComments = review.Comments.Count(), 
            Comments = review.Comments, 
            ReviewNote = review.ReviewNote
        };
    }
}
