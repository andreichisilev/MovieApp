using MovieAppUI.Areas.Admin.Models.DTOs;
using MovieAppUI.Models.Entities;

namespace MovieAppUI.Profiles
{
    public class GenreProfile: Profile
    {
        public GenreProfile()
        {
            CreateMap<NewGenreDto, Genre>();
            
            CreateMap<Genre, OldGenreDto>()
                .ForMember(d => d.GenreID, s => s.MapFrom(src => src.Id));
        }
    }
}
