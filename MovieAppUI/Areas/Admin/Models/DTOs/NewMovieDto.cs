using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Areas.Admin.Models.DTOs
{
    public class NewMovieDto
    {
        [MaxLength(200)]
        public string Title { get; set; }

        [Display(Name = "Release Date"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ReleaseDate { get; set; }

        //validate for positive number
        [Display(Name ="Runtime (minutes)"),Range(1, 1000)]
        public int Runtime { get; set; }

        [Display(Name = "Genre")]
        public int GenreID { get; set; }
    }
}
