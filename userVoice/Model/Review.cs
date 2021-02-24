using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using userVoice.Helper; 

using System.Threading.Tasks;

namespace userVoice.Model
{
    public class Review
    {
        [Required]
        public double Rate { get; set;  }
        [Required]
        public String Content { get; set;  }
        [Required]
        [JsonIgnore]
        public String AuthorId { get; set;  }
        public UserEntity Author { get; set;  }
        [Required]
        [JsonIgnore]
        public int ItemId { get; set;  }
        [JsonIgnore]
        public Item Item { get; set;  }
        [JsonConverter(typeof(DateConverter))]
        public DateTime CreatedAt { get; set; } = DateTime.Now; 
      
    }
}
