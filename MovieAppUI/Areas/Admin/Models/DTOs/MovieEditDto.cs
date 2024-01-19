using MovieAppUI.Areas.Admin.Models.ViewModels;

namespace MovieAppUI.Areas.Admin.Models.DTOs
{
    public class MovieEditDto
    {
        public OldMovieDto? OldMovie { get; set; }
        public List<ActorVm>? ActorList { get; set; }
    }
}
