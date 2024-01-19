using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAppUI.Models.Entities
{
    [Table("Actor")]
    public class Actor
    {
        [Column("ActorID")]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public ICollection<MovieActor>? Movies { get; set; }
        public ContactInfo? ContactInfo { get; set; }
    }
}
