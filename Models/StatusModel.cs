using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MangaCMS.Models
{
    public class StatusModel
    {
        [JsonIgnore]
        [Key]
        public int Id { get; set; }
        [Required]
        public string StatusName { get; set; }

        public virtual ICollection<MangaModel> Mangas { get; set; }

        public StatusModel()
        {
            Mangas = new List<MangaModel>();
        }
    }
}
