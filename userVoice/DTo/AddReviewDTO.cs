using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using userVoice.Helper;

namespace userVoice.DTo
{
    public class AddReviewDTO
    {
        [Required]
        [ModelBinder(BinderType = typeof(TypeBinder<int>) )]
        public int ItemId { get; set;  }
        [Required]
        [ModelBinder(BinderType = typeof(TypeBinder<String>))]
        public String AuthorId { get; set;  }
        [Required]
        public double Rate { get; set; }
        [Required]
        public String Content { get; set; }
    }
}
