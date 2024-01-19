using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAppUI.Models.Entities
{
    [Table("Movie_Actor")]
    [PrimaryKey(nameof(MovieID), nameof(ActorID))]
    public class MovieActor
    {
        public int MovieID { get; set; }
        [ForeignKey(nameof(MovieID))]
        public Movie? Movie { get; set; }
        public int ActorID { get; set; }
        [ForeignKey(nameof(ActorID))]
        public Actor? Actor { get; set;}
        public string Role { get; set; } = string.Empty;
    }
}
