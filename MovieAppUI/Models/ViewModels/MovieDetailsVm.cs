using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Models.ViewModels
{
    public class MovieDetailsVm
    {
        public int MovieID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Runtime { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ReleaseDate { get; set; }
        public List<CardActorVm> ActorsList { get; set; } = new List<CardActorVm>();
        public string Actors { get; set; } = string.Empty;
    }

}
