using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using userVoice.Helper;

namespace userVoice.DTo
{
    public class AddReviewDTO
    {
        [ModelBinder(BinderType = typeof(TypeBinder<int>) )]
        public int ItemId { get; set;  }
        [ModelBinder(BinderType = typeof(TypeBinder<String>))]
        public String AuthorId { get; set;  }
        public double Rate { get; set; }
        public String Content { get; set; }
    }
}
