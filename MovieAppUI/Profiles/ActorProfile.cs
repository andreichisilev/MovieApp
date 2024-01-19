using MovieAppUI.Areas.Admin.Models.DTOs;
using MovieAppUI.Areas.Admin.Models.ViewModels;
using MovieAppUI.Models.Entities;
using MovieAppUI.Models.ViewModels;

namespace MovieAppUI.Profiles
{
    public class ActorProfile: Profile
    {
       public ActorProfile()
        {
            CreateMap<NewActorDto, Actor>();

            CreateMap<Actor, ActorVm>()
                .ForMember(d => d.ActorID, s => s.MapFrom(src => src.Id))
                .ForMember(d => d.Name, s => s.MapFrom(src => $"{src.FirstName} {src.LastName} ({src.Nationality})"));
            
            CreateMap<Actor, ActorDetailsVm>()
                .ForMember(d => d.ActorID, s => s.MapFrom(src => src.Id))
                .ForMember(d => d.Name, s => s.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<Actor, OldActorDto>()
                .ForMember(d => d.ActorID, s => s.MapFrom(src => src.Id));

            CreateMap<MovieActor, ActorVm>()
                .ForMember(d => d.ActorID, s => s.MapFrom(src => src.Actor!.Id))
                .ForMember(d => d.Name, s=> s.MapFrom(src => $"{src.Actor!.FirstName} {src.Actor.LastName} ({src.Role})"));

            CreateMap<Actor, ContactInfoDto>()
                .ForMember(d => d.ActorID, s => s.MapFrom(src => src.Id))
                .ForMember(d => d.Email, s => s.MapFrom(src => src.ContactInfo!.Email))
                .ForMember(d => d.AgentPhoneNumber, s => s.MapFrom(src => src.ContactInfo!.AgentPhoneNumber))
                .ForMember(d => d.Name, s => s.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<Actor, ActorDto>()
                .ForMember(d => d.ActorID, s => s.MapFrom(src => src.Id))
                .ForMember(d => d.Name, s => s.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<Actor, CardActorVm>()
                .ForMember(d => d.ActorID, s => s.MapFrom(src => src.Id));

            CreateMap<Actor, ContactInfoVm>()
                .ForMember(d => d.ActorID, s => s.MapFrom(src => src.Id))
                .ForMember(d => d.Name, s => s.MapFrom(src => $"{src.FirstName} {src.LastName}"));
       }
    }
}
