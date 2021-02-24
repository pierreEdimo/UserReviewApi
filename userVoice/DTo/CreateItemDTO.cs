using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using userVoice.Helper;
using userVoice.Validation; 

namespace userVoice.DTo
{
    public class CreateItemDTO
    {
        [Required]
        [StringLength(80)]
        public String Name { get; set;  }
        [Required]
        [FileSizeValidator(4)]
        [ContentTypeValidator(ContentTypeGroup.Image)]
        public IFormFile Picture { get; set;  }
        public DateTime OpeningDate { get; set; }
        [Required]
        public String Description { get; set;  }
        [Required]
        [ModelBinder(BinderType = typeof(TypeBinder<int>))]
        public int GenreId { get; set; }
    }
}
