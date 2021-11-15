using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MangaCMS.Models
{
    public class MangaGenreModel
    {
        public int MangaId { get; set; }
        public MangaModel Manga { get; set; }

        public int GenreId { get; set; }
        public GenreModel Genre { get; set; }
    }
}
