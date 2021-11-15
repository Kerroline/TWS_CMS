using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MangaCMS.Models
{
    public class MangaModel
    {
        [JsonIgnore]
        [Key]
        public int Id { get; set; }

        [Required]
        public string JP_Name { get; set; }
        [Required]
        public string RU_Name { get; set; }
        [Required]
        public string ENG_Name { get; set; }
        [JsonIgnore]
        public string ContentDirPath { get; set; }
        [JsonIgnore]
        public string PosterPath { get; set; }          

        public string Link { get; set; }      
        
        public int Year { get; set; }           
        
        public string Author { get; set; }

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
