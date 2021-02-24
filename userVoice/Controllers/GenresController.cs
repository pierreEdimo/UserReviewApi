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
using userVoice.Services;
using userVoice.DTo;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace userVoice.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorage;
        private String containerName = "Genre"; 

        public GenresController(DatabaseContext context, 
                                IMapper mapper, 
                                IFileStorageService fileStorage )
        {
            _context = context;
            _mapper = mapper;
            _fileStorage = fileStorage; 
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<ActionResult<List<GenreDTO>>> GetGenres()
        {
            var genres =  await _context.Genres.ToListAsync();

            return _mapper.Map<List<GenreDTO>>(genres); 
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDTO>> GetGenre(int Id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == Id );

            if (genre == null)
            {
                return NotFound();
            }

            var genreDTO = _mapper.Map<GenreDTO>(genre); 

            return genreDTO;
        }

        [Authorize(Roles = "Admin")]
        // PUT: api/Genres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int Id, [FromForm] CreateGenreDTO updateGenre)
        {
            var genreDB = await _context.Genres.FirstOrDefaultAsync(x => x.Id == Id); 

            if(genreDB == null)
            {
                return NotFound(); 
            }

            genreDB = _mapper.Map(updateGenre, genreDB); 

            if( updateGenre.Picture != null)
            {
                using(var memoryStream = new MemoryStream())
                {
                    await updateGenre.Picture.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(updateGenre.Picture.FileName);
                    genreDB.Picture = await _fileStorage.EditFile(content, extension, containerName, genreDB.Picture, updateGenre.Picture.ContentType); 
                }
            }

            await _context.SaveChangesAsync(); 

            return NoContent();
        }

        // POST: api/Genres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> PostGenre( [FromForm]  CreateGenreDTO createGenre)
        {
            var genre = _mapper.Map<Genre>(createGenre); 

            if(createGenre.Picture != null)
            {
                using(var memoryStream = new MemoryStream())
                {
                    await createGenre.Picture.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(createGenre.Picture.FileName);
                    genre.Picture = await _fileStorage.SaveFile(content, extension, containerName, createGenre.Picture.ContentType); 
                }
            }

            _context.Add(genre);

            await _context.SaveChangesAsync();

            var genreDTO = _mapper.Map<GenreDTO>(genre); 

            return CreatedAtAction("GetGenre", new { id = genreDTO.Id }, genreDTO);
        }

        [Authorize(Roles = "Admin")]
        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int Id)
        {
            var exists = _context.Genres.AnyAsync(x => x.Id == Id); 
            if (!await exists )
            {
                return NotFound();
            }

            _context.Remove( new Genre() { Id = Id });

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
