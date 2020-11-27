using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using userVoice.OnlyDate;
using Newtonsoft.Json;


namespace userVoice.Model
{
    public class Item
    {
        [Key]
        public int Id { get; set;  }
        public string Name { get; set;  }
        public string Description { get; set;  }
        public string ImageUrl { get; set;  }
        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime EntryDate { get; set; } = DateTime.Now; 
        public int CategoryId { get; set;  }
        public virtual Category Category { get; set;  }
        public String Publisher { get; set;  }
        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime ReleaseDate { get; set;  }
        public virtual List<Review> Reviews { get; set;  }
        public String Genre { get; set;  }
      
    }
}
