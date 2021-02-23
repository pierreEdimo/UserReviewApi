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

namespace userVoice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchWordsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper; 

        public SearchWordsController(DatabaseContext context, 
                                     IMapper mapper)
        {
            _context = context;
            _mapper = mapper; 
        }

        // GET: api/SearchWords
        [HttpGet]
        public async Task<ActionResult<List<SearchWordDTO>>> GetSearchWords()
        {
            var words = await _context.SearchWords.ToListAsync();

            return _mapper.Map<List<SearchWordDTO>>(words); 
        }

        // GET: api/SearchWords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SearchWordDTO>> GetSearchWord(int Id)
        {
            var searchWord = await _context.SearchWords.FirstOrDefaultAsync(x=> x.Id == Id);

            if (searchWord == null)
            {
                return NotFound();
            }

            var wordDTO = _mapper.Map<SearchWordDTO>(searchWord); 

            return wordDTO;
        }

      
        // POST: api/SearchWords
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostSearchWord(AddSearchWord addsearchWord)
        {
            var searchWord = _mapper.Map<SearchWord>(addsearchWord);

            _context.Add(searchWord);
          
            await _context.SaveChangesAsync();

            var searchWordDTO = _mapper.Map<SearchWordDTO>(searchWord); 

            return CreatedAtAction("GetSearchWord", new { id = searchWordDTO.Id }, searchWordDTO );
        }

        // DELETE: api/SearchWords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSearchWord(int Id)
        {
            var exists = _context.SearchWords.AnyAsync(x => x.Id == Id); 

            if(!await exists)
            {
                return NotFound(); 
            }

            _context.Remove(new SearchWord() { Id = Id });

            await _context.SaveChangesAsync(); 

            return NoContent();
        }

     
    }
}
