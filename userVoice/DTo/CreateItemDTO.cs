using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using userVoice.Helper;

namespace userVoice.DTo
{
    public class CreateItemDTO
    {
        public String Name { get; set;  }
        public IFormFile Picture { get; set;  }
        public DateTime OpeningDate { get; set; }
        public String Description { get; set;  }
        [ModelBinder(BinderType = typeof(TypeBinder<int>))]
        public int GenreId { get; set; }
    }
}
