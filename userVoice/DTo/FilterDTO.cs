using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace userVoice.DTo
{
    public class FilterDTO
    {
        const int _maxSize = 100;

        private int _size = 50; 

        public int Size
        {
            get
            {
                return _size; 
            }
            set
            {
                _size = Math.Min(_maxSize, value); 
            }
        }

        public String sortBy { get; set; } = "Id";

        private String _sortOrder = "desc"; 

        public String SortOrder
        {
            get
            {
                return _sortOrder; 
            }
            set
            {
                if(value == "asc" || value == "desc")
                {
                    _sortOrder = value; 
                }
            }
        }
    }
}
