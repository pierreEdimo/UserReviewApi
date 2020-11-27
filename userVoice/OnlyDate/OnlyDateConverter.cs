using Newtonsoft.Json.Converters;

namespace userVoice.OnlyDate
{
    public class OnlyDateConverter: IsoDateTimeConverter
    {
        public OnlyDateConverter()
        {
            DateTimeFormat = "dd.MM.yyyy"; 
        }
    }
}
