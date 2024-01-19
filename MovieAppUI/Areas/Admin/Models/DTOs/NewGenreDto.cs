using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Areas.Admin.Models.DTOs
{
    public class NewGenreDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
