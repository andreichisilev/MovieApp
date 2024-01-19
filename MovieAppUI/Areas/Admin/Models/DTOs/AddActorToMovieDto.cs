using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Areas.Admin.Models.DTOs
{
    public class AddActorToMovieDto
    {
        public int MovieID { get; set; }
        public string Movie { get; set; } = string.Empty;

        [Display(Name = "Choose an actor:")]
        public int ActorID { get; set; }

        [Display(Name = "Role in movie:"), Required, MaxLength(50)]
        public string Role { get; set; } = string.Empty;
    }
}
