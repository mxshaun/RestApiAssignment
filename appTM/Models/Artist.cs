using System.ComponentModel.DataAnnotations;

namespace appTM.Models
{
    public class Artist
    {
        /// <summary>
        /// Artist_Id
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Artist_Name
        /// </summary>
        [Required]
        public string Name { get; set; }
        
    }
}
