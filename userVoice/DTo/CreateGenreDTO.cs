using Microsoft.AspNetCore.Http;
using System;


namespace userVoice.DTo
{
    public class CreateGenreDTO
    {
        public String Name { get; set;  }
        public IFormFile Picture { get; set;  }
    }
}
