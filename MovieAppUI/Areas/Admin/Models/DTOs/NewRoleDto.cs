using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Areas.Admin.Models.DTOs
{
    public class NewRoleDto
    {
        [Key]
        public string Name { get; set; } = string.Empty;
    }
}
