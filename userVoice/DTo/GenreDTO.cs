using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using userVoice.Model;

namespace userVoice.DTo
{
    public class GenreDTO
    {
        
        public int Id { get; set;  }
        public String Name { get; set;  }
        public String Picture { get; set;  }
        [JsonIgnore]
        public List<Item> Items { get; set;  }
    }
}
