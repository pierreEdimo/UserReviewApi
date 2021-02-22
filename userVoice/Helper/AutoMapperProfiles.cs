using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using userVoice.DTo;
using userVoice.Model; 

namespace userVoice.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Item, ItemDTO>().ReverseMap(); 
              
            CreateMap<CreateItemDTO, Item>()
                .ForMember(x => x.Picture, options => options.Ignore());

            CreateMap<Genre, GenreDTO>().ReverseMap();

            CreateMap<CreateGenreDTO, Genre>()
                .ForMember(x => x.Picture, options => options.Ignore());

            CreateMap<Review, ReviewDTO>().ReverseMap();

            CreateMap<AddReviewDTO, Review>(); 
        }


    }
}
