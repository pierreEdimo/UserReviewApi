using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using userVoice.Model;
using userVoice.OnlyDate;

namespace userVoice.DTo
{
    public class CategoryDTo
    {
        public string Name { get; set; }
        [Key]
        public int Id { get; set; }
        public virtual List<Item> Items { get; set; }
        public string ImageUrl { get; set;  }
        public int numberOfItems { get; set;  }
        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime EntryDate { get; set; } = DateTime.Now; 
    }
}
