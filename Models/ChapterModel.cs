using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.Models
{
    public class ChapterModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ChapterNumber { get; set; }

        [Required]
        public string ChapterName { get; set; }

        public ProgressModel Progress { get; set; }

        public int MangaId { get; set; }

        [ForeignKey("MangaId")]
        public MangaModel Manga { get; set; }
    }
}
