using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json; 
using System.Threading.Tasks;

namespace userVoice.Model
{
    public class Genre
    {
       [Key]
        public int Id { get; set;  }
        public String Name { get; set;  }
        public String Picture { get; set;  }
        [JsonIgnore]
        public List<Item> Items { get; set;  }

    }
}
