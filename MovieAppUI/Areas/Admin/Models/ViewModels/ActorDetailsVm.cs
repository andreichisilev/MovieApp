using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Areas.Admin.Models.ViewModels
{
    public class ActorDetailsVm
    {
        public int ActorID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;

        [Display(Name = "Birth Date"), DisplayFormat(DataFormatString = "{0:d MMMM yyyy}")]
        public DateTime BirthDate { get; set; }
    }
}
