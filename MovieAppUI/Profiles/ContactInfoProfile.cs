using MovieAppUI.Areas.Admin.Models.DTOs;
using MovieAppUI.Models.Entities;

namespace MovieAppUI.Profiles
{
    public class ContactInfoProfile: Profile
    {
       public ContactInfoProfile()
        {
            CreateMap<NewContactInfoDto, ContactInfo>();
        }
    }
}
