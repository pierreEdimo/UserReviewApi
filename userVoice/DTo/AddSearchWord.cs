﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace userVoice.DTo
{
    public class AddSearchWord
    {
        [Required]
        public String UserId { get; set; }
        [Required]
        public String Word { get; set; }
    }
}