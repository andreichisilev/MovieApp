using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Models.ViewModels
{
    public class ContactInfoVm
    {
        public int ActorID { get; set; }

        [Display(Name = "Actor Name")]
        public required string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [Display(Name = "Agent Phone Number")]
        public string AgentPhoneNumber { get; set; } = string.Empty;
    }
}
