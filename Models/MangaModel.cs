using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.Models
{
    public class MangaModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string OrigName { get; set; }
        [Required]
        public string RuName { get; set; }
        [Required]
        public string EngName { get; set; }
        public string Poster { get; set; }          // Картинка 

        [Required]
        public string Link { get; set; }            // Ссылка
        [Required]
        public int Year { get; set; }               // Год  
        [Required]
        public string Author { get; set; }          // Автор
        [Required]
        public string Description { get; set; }    // Описание

        public int StatusId { get; set; } // Внешний ключ для связи со статусами

        [ForeignKey("StatusId")]
        public virtual StatusModel Status { get; set; }     // Статус 


        //public virtual MangasUsers MangasUsers { get; set; }

        public virtual ICollection<MangaGenreModel> MangasGenres { get; set; }  // Жанры

        public virtual ICollection<ChapterModel> Chapters { get; set; } // Главы

        public MangaModel()
        {
            MangasGenres = new List<MangaGenreModel>();
            Chapters = new List<ChapterModel>();
        }
    }
}
