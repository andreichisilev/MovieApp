using MovieAppUI.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Areas.Admin.Models.DTOs
{
    public class OldMovieDto
    {
        public int MovieID { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        [Display(Name = "Release Date"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ReleaseDate { get; set; }

        //validate for positive number
        [Display(Name = "Runtime (minutes)"), Range(1, 1000)]
        public int Runtime { get; set; }

        [Display(Name = "Genre")]
        public int GenreID { get; set; }
        
        public void ToEntity(ref Movie OldMovie)
        {
            if (OldMovie.Id == MovieID)
            {
                OldMovie.Title = Title;
                OldMovie.ReleaseDate = ReleaseDate;
                OldMovie.Runtime = Runtime;
                OldMovie.GenreID = GenreID;
            }
        }
    }
}
