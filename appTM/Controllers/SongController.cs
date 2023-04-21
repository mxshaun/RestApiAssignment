using appTM.Data;
using appTM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appTM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly MusicDbContext _context;

        public SongController(MusicDbContext context) => _context = context;


        [HttpGet]
        public async Task<IEnumerable<Song>> Get()
        {
            return await _context.Songs.ToListAsync(); ;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Song), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var song = await _context.Artists.FindAsync(id);
            return song == null ? NotFound() : Ok(song);
        }

        [HttpGet("ByGenre/{genre}")]
        [ProducesResponseType(typeof(Artist), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByGenreAsync(string genre)
        {
            var song = await _context.Songs.Where(p => p.Genre.Equals(genre)).ToListAsync();


            return song == null ? NotFound() : Ok(song);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(Song song)
        {
            // Check wether artist name already exists
            var item = _context.Artists.Where(p => p.Name.Equals(song.Name)).FirstOrDefault();
            if (item != null) return BadRequest(item);


            await _context.Songs.AddAsync(song);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = song.Id }, song);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, Song song)
        {
            if (id != song.Id) return BadRequest();

            _context.Entry(song).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var songToDelete = await _context.Songs.FindAsync(id);
            if (songToDelete == null) return BadRequest();

            _context.Songs.Remove(songToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }




    }
}
