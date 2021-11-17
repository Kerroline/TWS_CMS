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
        public int ID { get; set; }

        [Required]
        public string japanName { get; set; }
        [Required]
        public string russianName { get; set; }
        [Required]
        public string englishName { get; set; }
        

        public string link { get; set; }      
        
        public int year { get; set; }           
        
        public string author { get; set; }

        public string description { get; set; }    

        public int statusID { get; set; } 

        [ForeignKey("statusID")]
        public virtual StatusModel status { get; set; }     

        public int fileID { get; set; }

        [ForeignKey("fileID")]
        public virtual FileModel poster { get; set; }


        //public virtual MangasUsers MangasUsers { get; set; }

        public virtual ICollection<MangaGenreModel> listOfGenres { get; set; }  

        public virtual ICollection<ChapterModel> listOfChapters { get; set; } 

        public MangaModel()
        {
            listOfGenres = new List<MangaGenreModel>();
            listOfChapters = new List<ChapterModel>();
        }
    }
}
