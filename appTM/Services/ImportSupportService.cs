using appTM.Models;
using appTM.Resources;

namespace appTM.Services
{
    public class ImportSupportService
    {
        private readonly HttpClient client = new();
        private const string ARTISTJSONURL = "https://raw.githubusercontent.com/Team-Rockstars-IT/MusicLibrary/v1.0/artists.json";
        private const string SONGJSONURL = "https://raw.githubusercontent.com/Team-Rockstars-IT/MusicLibrary/v1.0/songs.json";



        public IEnumerable<Artist> CreateListOfArtists()
        {
            // Download Json File
            var artistsJsonData = DownloadArtistsJsonDataAsync();

            // Deserialize
            var listOfArtists = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Artist>>(artistsJsonData.Result);

            // Check
            if (listOfArtists == null) throw new Exception("");

            foreach (var artist in listOfArtists)
            {
                artist.Id = 0;
            }
            return listOfArtists;

        }

        public async Task<IEnumerable<Song>> CreateListOfSongs()
        {
            // Download Json File
            var songsJsonData = await DownloadSongsJsonDataAsync();

            // Deserialize
            var listOfSongs = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Song>>(songsJsonData);

            // Check
            if (listOfSongs == null) throw new Exception("");

            // Request: Only songs before 2016 and Genre Metal
            var listOfSongsBefore2016AndGenreMetal = listOfSongs.Where(s => s.Year < 2016 && s.Genre.Equals("Metal"));

            foreach (var song in listOfSongsBefore2016AndGenreMetal)
            {
                song.Id = 0;
            }
            return listOfSongsBefore2016AndGenreMetal;
        }

        private async Task<string> DownloadArtistsJsonDataAsync()
        {
            try
            {
                
                var myArtistJson = await client.GetStringAsync(Resource.ArtistJsonURL);
                return myArtistJson;
            }
            catch (Exception)
            {
                throw;
            }

        }
        private async Task<string> DownloadSongsJsonDataAsync()
        {
            try
            {
                var mySongJson = await client.GetStringAsync(Resource.SongsJsonURL);
                return mySongJson;
            }
            catch (Exception)
            {
                throw;
            }

        }


    }
}
