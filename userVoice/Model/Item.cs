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
        public String Name { get; set;  }
        public String Picture { get; set;  }
        public DateTime OpeningDate { get; set; }
        [JsonIgnore]
        public List<Review> Reviews { get; set;  }
        public Genre Genre { get; set;  } 
        public String Description { get; set;  }
        public int GenreId { get; set; }
        public double Rating { get; set;  }
    }
}
