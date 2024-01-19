using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Areas.Admin.Models.ViewModels
{
    public class ActorVm
    {
        public int ActorID { get; set; }

        [Display(Name="Actors")]
        public string Name { get; set; } = string.Empty;
    }
}
