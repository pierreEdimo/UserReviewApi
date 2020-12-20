﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace userVoice.Model
{
    public class UserEntity : IdentityUser
    {
        public String adress { get; set; }

        public virtual List<Review> getReviews {get; set; }

        public virtual List<Comment> getComments { get; set;  }

    }
}