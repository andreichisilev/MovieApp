using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Areas.Admin.Models.DTOs
{
    public class ContactInfoDto
    {
        public int ActorID { get; set; }

        [Display(Name = "Actor Name")]
        public required string Name { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        [Display(Name = "Agent Phone Number"), Required, MaxLength(15)]
        public string AgentPhoneNumber { get; set; } = string.Empty;
        public bool IsNew { get; set; } = true;
    }
}
