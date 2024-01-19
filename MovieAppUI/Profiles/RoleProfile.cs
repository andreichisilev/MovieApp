using MovieAppUI.Models.CustomIdentity;
using MovieAppUI.Areas.Admin.Models.DTOs;
using MovieAppUI.Areas.Admin.Models.ViewModels;

namespace MovieAppUI.Profiles
{
    public class RoleProfile: Profile
    {
        public RoleProfile()
        {
            CreateMap<AppRole, NewRoleDto>();
            CreateMap<AppRole, ExistingRoleDto>();
        }

    }
}
