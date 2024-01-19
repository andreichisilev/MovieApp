using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Areas.Admin.Models.DTOs
{
    public class ExistingUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        
        [Display(Name = "Admin?")]
        public bool IsAdmin { get; set; } = false;
    }
}
