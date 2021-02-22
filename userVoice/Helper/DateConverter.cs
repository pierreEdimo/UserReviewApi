using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace userVoice.Helper
{
    public class DateConverter : IsoDateTimeConverter
    {
        public DateConverter()
        {
            DateTimeFormat = "dd.MM.yyyy"; 
        }
    }
}
