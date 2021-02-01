using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using userVoice.Model;
using userVoice.OnlyDate;

namespace userVoice.DTo
{
    public class ItemDTo
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime EntryDate { get; set; } = DateTime.Now;
        public int CategoryId { get; set; }
        public virtual Category Category { get; set;  }
        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime ReleaseDate { get; set; }
        public virtual List<Review> Reviews { get; set; }
        public int numberOfReviews { get; set;  }
        public int Note{get; set; }
    }
}
