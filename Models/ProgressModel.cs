using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.Models
{
    public class ProgressModel
    {
        [Key]
        public int Id { get; set; }

        public bool Cleaner { get; set; }
        public bool Translator { get; set; }
        public bool Сorrector { get; set; }
        public bool Tiper { get; set; }
        public bool Editor { get; set; }

        public bool Complite
        {
            get
            { return (Cleaner && Translator && Сorrector && Tiper && Editor); }
        }

        public int ChapterId { get; set; }

        [ForeignKey("ChapterId")]
        public ChapterModel Chapter { get; set; }

    }
}
