using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MangaCMS.Models
{
    public class FileModel
    {
        [JsonIgnore]
        [Key]
        public int Id { get; set; }

        [JsonIgnore]
        public string Path { get; set; }

    }
}
