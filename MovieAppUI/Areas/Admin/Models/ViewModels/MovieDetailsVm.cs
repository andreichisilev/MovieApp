using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Areas.Admin.Models.ViewModels
{
    public class MovieDetailsVm
    {
        public int MovieID { get; set; }

        public string Title { get; set; } = string.Empty;

        [Display(Name = "Release Date"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ReleaseDate { get; set; }

        public string Runtime { get; set; } = string.Empty;

        public string Genre { get; set; } = string.Empty;

        public required string Actors { get; set; }

    }
}
