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
        public string JP_Name { get; set; }
        [Required]
        public string RU_Name { get; set; }
        [Required]
        public string ENG_Name { get; set; }

        public string ContentDirPath { get; set; }

        public string PosterPath { get; set; }          

        [Required]
        public string Link { get; set; }            
        [Required]
        public int Year { get; set; }               
        [Required]
        public string Author { get; set; }          
        [Required]
        public string Description { get; set; }    

        public int StatusId { get; set; } 

        [ForeignKey("StatusId")]
        public virtual StatusModel Status { get; set; }     


        //public virtual MangasUsers MangasUsers { get; set; }

        public virtual ICollection<MangaGenreModel> MangasGenres { get; set; }  

        public virtual ICollection<ChapterModel> Chapters { get; set; } 

        public MangaModel()
        {
            MangasGenres = new List<MangaGenreModel>();
            Chapters = new List<ChapterModel>();
        }
    }
}
