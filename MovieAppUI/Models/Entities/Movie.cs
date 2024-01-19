using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAppUI.Models.Entities
{
    [Table("Movie")]
    public class Movie
    {
        [Column("MovieID")]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public int Runtime { get; set; }

        public int GenreID { get; set; }
        [ForeignKey(nameof(GenreID))]
        public Genre? Genre { get; set; }

        public ICollection<MovieActor>? Actors { get; set; }
    }
}
