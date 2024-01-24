using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAppUI.Models.Entities
{
    [Table("ContactInfo")]
    public class ContactInfo
    {
        [Column("ActorID")]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public Actor? Actor { get; set; }

        [MaxLength(100),EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [MaxLength(15),Phone]
        public string AgentPhoneNumber { get; set; } = string.Empty;

    }
}
