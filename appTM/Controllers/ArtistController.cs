using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using appTM.Data;
using appTM.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace appTM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly MusicDbContext _context;

        public ArtistController(MusicDbContext context) => _context = context;


        [HttpGet]
        public async Task<IEnumerable<Artist>> Get()
        {
            return await _context.Artists.ToListAsync(); ;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Artist), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            return artist == null ? NotFound() : Ok(artist);
        }

        [HttpGet("ByName/{name}")]
        [ProducesResponseType(typeof(Artist), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByName(string name)
        {
            var artist = _context.Artists.Where(p => p.Name.Equals(name)).FirstOrDefault();
            return artist == null ? NotFound() : Ok(artist);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(Artist artist)
        {
            // Voorwaarde: Het kunnen toevoegen van nieuwe band(s) of nummers met alle details die ook in
            // de JSON staan waarbij de naam natuurlijk niet dubbel voor mag komen
            var item = _context.Artists.Where(p => p.Name.Equals(artist.Name)).FirstOrDefault(); 
            if (item != null) return BadRequest(item);

            // Dit kan ook
            //foreach (var artiest in _context.artists)
            //{
            //    if (artiest.Name.Equals(artist.Name)) return BadRequest(artiest);
            //}
                
            await _context.Artists.AddAsync(artist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new {id = artist.Id}, artist);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, Artist artist)
        {
            if (id != artist.Id) return BadRequest();

            _context.Entry(artist).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var artistToDelete = await _context.Artists.FindAsync(id);
            if (artistToDelete == null) return BadRequest();

            _context.Artists.Remove(artistToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }



    }
}
