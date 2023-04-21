using appTM.Data;
using appTM.Models;
using appTM.Services;
using Microsoft.AspNetCore.Mvc;

namespace appTM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly MusicDbContext context;
        private readonly HttpClient client = new();
        private ImportSupportService importSupportService;

        public ImportController(MusicDbContext context)
        {
            this.context = context;
            importSupportService = new ImportSupportService();
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create()
        {

            var listOfArtists = importSupportService.CreateListOfArtists();
            listOfArtists = CheckArtistsForDuplicates(listOfArtists);

            var listOfSongs = importSupportService.CreateListOfSongs();
            var listOfSongsResult = CheckSongsForDuplicates(listOfSongs.Result);

            context.Songs.UpdateRange(listOfSongsResult);
            context.Artists.UpdateRange(listOfArtists);
            await context.SaveChangesAsync();

            return NoContent();
        }



        private IEnumerable<Song> CheckSongsForDuplicates(IEnumerable<Song> listOfSongsBefore2016AndGenreMetal)
        {
            var songs = context.Songs.ToList();
            songs.AddRange(listOfSongsBefore2016AndGenreMetal);
            return songs.DistinctBy(x => x.Name);
        }

        private IEnumerable<Artist> CheckArtistsForDuplicates(IEnumerable<Artist> listOfArtists)
        {
            var artists = context.Artists.ToList();
            artists.AddRange(listOfArtists);
            return artists.DistinctBy(x => x.Name);
        }

    }
}