using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace userVoice.DTo
{
    public class FilterReviewDTO : FilterDTO
    {
        public String AuthorId { get; set; }
        public int ItemId { get; set; }
    }
}
