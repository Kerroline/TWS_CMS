using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MangaCMS.Models
{
    public class GenreModel
    {
        [JsonIgnore]
        [Key]
        public int Id { get; set; }
        [Required]
        public string GenreName { get; set; }
        public virtual ICollection<MangaGenreModel> MangasGenres { get; set; }

        public GenreModel()
        {
            MangasGenres = new List<MangaGenreModel>();
        }
    }
}
