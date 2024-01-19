using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Models.ViewModels
{
    public class CardActorVm
    {
        [Key]
        public int ActorID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;

    }
}
