using System;
using System.Collections.Generic;
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
