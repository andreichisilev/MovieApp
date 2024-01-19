using MovieAppUI.Areas.Admin.Models.DTOs;
using MovieAppUI.Models.CustomIdentity;

namespace MovieAppUI.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, ExistingUserDto>()
                .ForMember(d => d.Id, s => s.MapFrom(src => src.Id))
                .ForMember(d => d.Email, s => s.MapFrom(src => src.UserName));

        }
    }
}
