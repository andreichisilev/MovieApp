using MovieAppUI.Models.Entities;
using MovieAppUI.Validators;
using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Areas.Admin.Models.DTOs
{
    public class OldActorDto
    {
        public int ActorID { get; set; }

        [MaxLength(50), Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50), Display(Name = "Last Name"), Required]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Nationality { get; set; } = string.Empty;
        [Required,Display(Name = "Birth Date"), BirthDate, DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime BirthDate { get; set; }

        public void ToEntity(ref Actor OldActor)
        {
            if(OldActor.Id == ActorID)
            {
                OldActor.FirstName = FirstName;
                OldActor.LastName = LastName;
                OldActor.Nationality = Nationality;
                OldActor.BirthDate = BirthDate;
            }
        }

    }
}
