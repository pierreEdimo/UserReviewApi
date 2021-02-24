using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using userVoice.Validation; 

namespace userVoice.DTo
{
    public class CreateGenreDTO
    {
        [Required]
        public String Name { get; set;  }
        [Required]
        [FileSizeValidator(4)]
        [ContentTypeValidator(ContentTypeGroup.Image)]
        public IFormFile Picture { get; set;  }
    }
}
