using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.Models
{
    public class StatusModel
    {
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
