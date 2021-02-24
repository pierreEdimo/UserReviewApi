using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json; 

namespace userVoice.Model
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(80)]
        public String Name { get; set;  }
        [Required]
        public String Picture { get; set;  }
        [Required]
        public DateTime OpeningDate { get; set; }
        [JsonIgnore]
        public List<Review> Reviews { get; set;  }
        public Genre Genre { get; set;  } 
        [Required]
        public String Description { get; set;  }
        [Required]
        public int GenreId { get; set; }
        [Required]
        public double Rating { get; set;  }
    }
}
