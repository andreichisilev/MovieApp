using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppUI.Data;
using MovieAppUI.Models.ViewModels;

namespace MovieAppUI.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public MovieController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [Route("Movies")]
        public async Task<IActionResult> Index()
        {
            var Movies = await _context.Movies
                .Include(t => t.Actors!)
                    .ThenInclude(t => t.Actor)
                .Include(t => t.Genre)
                .OrderBy(i => i.Title)
                .ToListAsync();
            Console.WriteLine(Movies);

            return View(_mapper.Map<List<CardMovieVm>>(Movies));
        }

        [Route("Movie/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var Movie = _mapper.Map<MovieDetailsVm>(await _context.Movies
                .Include(t => t.Genre)
                .FirstOrDefaultAsync(i => i.Id == id));
            if (Movie == null)
                return NotFound();
            var Actors = _mapper.Map<List<CardActorVm>>(await _context.Actors
                .Where(i => i.Movies!.Any(i => i.MovieID == id))
                .ToListAsync());
            Movie.ActorsList = Actors;
            return View(Movie);
        }
    }
}
