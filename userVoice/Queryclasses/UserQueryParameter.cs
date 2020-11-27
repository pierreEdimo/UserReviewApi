using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace userVoice.Queryclasses
{
    public class UserQueryParameter:QueryParameters
    {   
        public string Name { get; set;}

        public string authorId { get; set;}

        public int itemId{get; set;}

        public int categoryId { get; set;  }

        public int reviewId { get; set;  }
    }
}
