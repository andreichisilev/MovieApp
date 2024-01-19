using MovieAppUI.Validators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace MovieAppUI.Areas.Admin.Models.DTOs
{
    public class NewActorDto
    {
        [MaxLength(50, ErrorMessage = "{0} must have maximum {1} characters."), Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "{0} must have maximum {1} characters."), Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "{0} must have maximum {1} characters.")]
        public string Nationality { get; set; } = string.Empty;
        [Display(Name = "Birth Date"), BirthDate, DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime BirthDate { get; set; }
    }
}
