using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using userVoice.OnlyDate;

namespace userVoice.Model
{
    public class Comment
    {
        public int Id { get; set;  }
        public int ReviewId { get; set;  }
        public virtual Review Review { get; set;  }
        public string Body { get; set;  }
        public string AuthorId { get; set;  }
        public virtual UserEntity Author { get; set;  }
        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime EntryDate { get; set; } = DateTime.Now; 
    
    }
}
