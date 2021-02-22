using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using userVoice.Helper;
using userVoice.Model; 

namespace userVoice.DTo
{
    public class ItemDTO
    {
        public int Id { get; set;  }
        public String Name { get; set;  }
        public String Picture { get; set;  }
        [JsonConverter( typeof(DateConverter) )]
        public DateTime OpeningDate { get; set; }
        public String Description { get; set;  }
        public GenreDTO Genre { get; set;  }

        public double Rating { get; set; }
        public List<Review> Reviews { get; set; }




    }
}
