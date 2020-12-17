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
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public int ItemId { get; set; }
        public string Body { get; set; }
        public virtual Item Item { get; set;  }
        public virtual List<Comment> Comments { get; set; }
        public int numberOfComments { get; set;  }
        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime EntryDate { get; set;  }
        public int ReviewNote{get; set; }
    }
}
