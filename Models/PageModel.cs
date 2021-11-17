using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MangaCMS.Models
{
    public class PageModel
    {
        [JsonIgnore]
        [Key]
        public int Id { get; set; }

        [Required]
        public int PageNumber { get; set; }

        public int FileId { get; set; }

        [ForeignKey("FileId")]
        public virtual FileModel File { get; set; }

        public int ChapterId { get; set; }

        [ForeignKey("ChapterId")]
        public virtual ChapterModel Chapter { get; set; }
    }
}
