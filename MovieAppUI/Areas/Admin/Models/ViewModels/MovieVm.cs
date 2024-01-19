using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Areas.Admin.Models.ViewModels
{
    public class MovieVm
    {
        public int MovieID { get; set; }
        [Display(Name = "Titles")]
        public string Title { get; set; } = string.Empty;
    }
}
