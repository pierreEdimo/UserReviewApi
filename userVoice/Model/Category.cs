using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using userVoice.OnlyDate;

namespace userVoice.Model
{
    public class Category
    {
        public string Name { get; set;  }
        [Key]
        public int Id { get; set;}
        public virtual List<Item> Items { get; set;  }
        public string ImageUrl { get; set;  }
        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime EntryDate { get; set; } = DateTime.Now; 
        
    }
}
