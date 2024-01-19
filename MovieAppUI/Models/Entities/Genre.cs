using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAppUI.Models.Entities
{
    [Table("Genre")]
    public class Genre
    {
        [Column("GenreID")]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Movie>? Movies { get; set; }
    }
}
