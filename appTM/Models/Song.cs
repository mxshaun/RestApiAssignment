using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace appTM.Models
{
    public class Song
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Year")]
        public int Year { get; set; }

        [JsonPropertyName("Artist")]
        public string Artist { get; set; }

        [JsonPropertyName("Shortname")]
        public string Shortname { get; set; }

        [JsonPropertyName("Bpm")]
        public int? Bpm { get; set; }

        [JsonPropertyName("Duration")]
        public int Duration { get; set; }

        [JsonPropertyName("Genre")]
        public string Genre { get; set; }

        [JsonPropertyName("SpotifyId")]
        public string? SpotifyId { get; set; }

        [JsonPropertyName("Album")]
        public string? Album { get; set; }
    }
}
