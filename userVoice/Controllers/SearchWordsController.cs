 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using userVoice.DBContext;
using userVoice.Model;
using userVoice.DTo;
using userVoice.Queryclasses; 

namespace userVoice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchWordsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public SearchWordsController(DatabaseContext context)
        {
            _context = context;

            _context.Database.EnsureCreated();
        }

        // GET: api/SearchWords
        [HttpGet(Name = nameof(GetSearchWords))]
        public async Task<ActionResult<IEnumerable<SearchWordDTo>>> GetSearchWords([FromQuery] UserQueryParameter queryParameters)
        {
            IQueryable<SearchWord> searchWords = _context.searchWords;

            if (!string.IsNullOrEmpty(queryParameters.sortBy))
            {
                if (typeof(SearchWord).GetProperty(queryParameters.sortBy) != null)
                {
                    searchWords = searchWords.OrderByCustom(queryParameters.sortBy, queryParameters.SortOrder);
                }
            }

            if (!string.IsNullOrEmpty(queryParameters.userId ))
            {
                searchWords = searchWords.Where(p => p.userId.Contains(queryParameters.userId));
            }

            return await searchWords.Select(x => searchWordToDTo(x)).ToListAsync();
        }

        // GET: api/SearchWords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SearchWord>> GetSearchWord(int id)
        {
            var searchWord = await _context.searchWords.FindAsync(id);

            if (searchWord == null)
            {
                return NotFound();
            }

            return searchWord;
        }



        // POST: api/SearchWords
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SearchWordDTo>> PostSearchWord(SearchWordDTo searchWordDTo)
        {
            var searchWord = new SearchWord
            {
                KeyWord = searchWordDTo.KeyWord, 
                userId = searchWordDTo.userId
            };


            _context.searchWords.Add(searchWord);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSearchWord), new { id = searchWord.Id }, searchWord);
        }

        // DELETE: api/SearchWords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSearchWord(int id)
        {
            var searchWord = await _context.searchWords.FindAsync(id);
            if (searchWord == null)
            {
                return NotFound();
            }

            _context.searchWords.Remove(searchWord);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        public static SearchWordDTo searchWordToDTo(SearchWord searchWord) => new SearchWordDTo
        {
            Id = searchWord.Id,
            KeyWord = searchWord.KeyWord,
            userId = searchWord.userId
        };
    }
}

