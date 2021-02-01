using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using userVoice.Model;
using userVoice.OnlyDate;

namespace userVoice.DTo
{
    public class ReviewDTo
    {
  
        public string AuthorId { get; set; }
        public virtual UserEntity Author { get; set;  }
        public int ItemId { get; set; }
        public string Body { get; set; }
        public virtual Item Item { get; set;  }
   
        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime EntryDate { get; set;  } = DateTime.Now;
        public int ReviewNote{get; set; }
    }
}
