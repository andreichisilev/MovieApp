using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Areas.Admin.Models.DTOs
{
    public class ActorDto
    {
        public int ActorID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
