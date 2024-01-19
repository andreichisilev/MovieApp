using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Models.ViewModels
{
    public class CardMovieVm
    {
        public int MovieID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;

        public string Actors { get; set; } = string.Empty;
        [DisplayFormat(DataFormatString = "{0:d MMMM yyyy}")]
        public string ReleaseDate { get; set; }

        public string Runtime { get; set; } = string.Empty;

    }
}
