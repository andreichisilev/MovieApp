using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Areas.Admin.Models.DTOs
{
    public class NewContactInfoDto
    {
        [MaxLength(50, ErrorMessage = "{0} must have maximum {1} characters."), Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "{0} must have maximum {1} characters."), Display(Name = "Agent Phone Number")]
        public string AgentPhoneNumber { get; set; } = string.Empty;
    }
}
