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
    public class CommentsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CommentsController(DatabaseContext context)
        {
            _context = context;

            _context.Database.EnsureCreated();
        }

        // GET: api/Comments
        [HttpGet(Name = nameof(Getcomments))]
        public async Task<ActionResult<IEnumerable<CommentDTo>>> Getcomments([FromQuery] UserQueryParameter queryParameter )
        {
            IQueryable<Comment> comments = _context.comments;

            if (!string.IsNullOrEmpty(queryParameter.sortBy))
            {
                if (typeof(Comment).GetProperty(queryParameter.sortBy) != null)
                {
                    comments = comments.OrderByCustom(queryParameter.sortBy, queryParameter.SortOrder);
                }
            }

            if (!string.IsNullOrEmpty(queryParameter.reviewId.ToString()))
            {
                comments = comments.Where(p => p.ReviewId.ToString().Contains(queryParameter.reviewId.ToString())); 
            }

            return await comments.Include(a => a.Author)
                                 .Include(a => a.Review)
                                 .Select(x => CommentToDTo(x)).ToListAsync(); 
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDTo>> GetComment(int id)
        {
            IQueryable<Comment> comments = _context.comments;

            var comment = await comments.Include(a => a.Review).FirstOrDefaultAsync(a => a.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            return CommentToDTo(comment);
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, CommentDTo commentDTo)
        {
            if (id != commentDTo.Id)
            {
                return BadRequest();
            }

            var comment = await _context.comments.FindAsync(id); 

            if(comment == null)
            {
                return NotFound(); 
            }

            comment.Body = commentDTo.Body; 

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // POST: api/Comments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CommentDTo>> PostComment(CommentDTo commentDTo)
        {
            var comment = new Comment
            {
                AuthorId = commentDTo.AuthorId,
                Body = commentDTo.Body,
                ReviewId = commentDTo.ReviewId
            }; 

            _context.comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, CommentToDTo(comment));
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            var comment = await _context.comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(int id)
        {
            return _context.comments.Any(e => e.Id == id);
        }

        private static CommentDTo CommentToDTo(Comment comment) => new CommentDTo
        {
            Id = comment.Id,
            AuthorId = comment.AuthorId,
            Body = comment.Body,
            EntryDate = comment.EntryDate,
            Review = comment.Review,
            ReviewId = comment.ReviewId, 
            Author = comment.Author
        }; 
    }
}
