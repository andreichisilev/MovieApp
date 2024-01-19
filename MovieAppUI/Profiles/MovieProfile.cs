using MovieAppUI.Areas.Admin.Models.DTOs;
using MovieAppUI.Areas.Admin.Models.ViewModels;
using MovieAppUI.Models.Entities;
using MovieAppUI.Models.ViewModels;

namespace MovieAppUI.Profiles
{
    public class MovieProfile: Profile
    {
        public MovieProfile()
        {
            CreateMap<NewMovieDto, Movie>();

            CreateMap<Movie, MovieVm>()
                .ForMember(d => d.MovieID, s => s.MapFrom(src => src.Id))
                .ForMember(d => d.Title, s => s.MapFrom(src => $"{src.Title}({src.ReleaseDate.Year})"));

            CreateMap<Movie, OldMovieDto>()
                .ForMember(d => d.MovieID, s => s.MapFrom(src => src.Id));

            CreateMap<Movie, Areas.Admin.Models.ViewModels.MovieDetailsVm>()
                .ForMember(d => d.MovieID, s => s.MapFrom(src => src.Id))
                .ForMember(d => d.Genre, s => s.MapFrom(src => src.Genre!.Name))
                .ForMember(d => d.Runtime, s => s.MapFrom(src => $"{src.Runtime} minutes"))
                .ForMember(d => d.Actors, s => s.MapFrom(src => string.Join(", ", src.Actors!.Select(a => $"{a.Actor!.FirstName} {a.Actor!.LastName}"))));

            CreateMap<Movie, AddActorToMovieDto>()
                .ForMember(d => d.MovieID, s => s.MapFrom(src => src.Id))
                .ForMember(d => d.Movie, s => s.MapFrom(src => $"{src.Title}({src.ReleaseDate.Year})"));

            CreateMap<Movie, CardMovieVm>()
                .ForMember(d => d.MovieID, s => s.MapFrom(src => src.Id))
                .ForMember(d => d.Genre, s => s.MapFrom(src => src.Genre!.Name))
                .ForMember(d => d.Runtime, s => s.MapFrom(src => $"{src.Runtime} minutes"))
                .ForMember(d => d.ReleaseDate, s => s.MapFrom(src => src.ReleaseDate.ToString("d MMMM yyyy")))
                .ForMember(d => d.Actors, s => s.MapFrom(src => string.Join(", ", src.Actors!.Select(a => $"{a.Actor!.FirstName} {a.Actor!.LastName}"))));

            CreateMap<Movie, Models.ViewModels.MovieDetailsVm>()
                .ForMember( d => d.MovieID, s => s.MapFrom(src => src.Id))
                .ForMember(d => d.Genre, s => s.MapFrom(src => src.Genre!.Name))
                .ForMember(d => d.Runtime, s => s.MapFrom(src => $"{src.Runtime} minutes"))
                .ForMember(d => d.Actors, s => s.MapFrom(src => string.Join(", ", src.Actors!.Select(a => $"{a.Actor!.FirstName} {a.Actor!.LastName}"))));

        }
    }
}
