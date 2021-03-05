using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using userVoice.Helper;
using userVoice.Model;

namespace userVoice.DTo
{
    public class ReviewDTO
    {
        public String AuthorId { get; set;  }
        public double Rate { get; set;  }
        public String Content { get; set;  }
        [JsonIgnore]
        public ItemDTO Item { get; set;  }
        public UserDTO Author { get; set;  }
        [JsonConverter(typeof(DateConverter) )]
        public DateTime CreatedAt { get; set; } = DateTime.Now; 
        public int ItemId { get; set;  }
    }
}
