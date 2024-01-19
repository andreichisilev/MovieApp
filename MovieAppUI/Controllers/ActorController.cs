using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppUI.Data;
using MovieAppUI.Models.ViewModels;

namespace MovieAppUI.Controllers
{
    public class ActorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ActorController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Actor
        [Route("Actors")]
        public async Task<IActionResult> Index()
        {
            /*return _context.Actors != null ? 
                        View(await _context.Actors.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Actors'  is null.");*/
            if (_context.Actors == null)
                return Problem("Entity set 'ApplicationDbContext.Actors'  is null.");
            var actors = await _context.Actors
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .ToListAsync();
            return View(_mapper.Map<List<CardActorVm>>(actors));

        }

        // GET: Actor/Details/5
        [Route("Actors/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
           var Actor = await _context.Actors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Actor == null)
                return NotFound();

            var Movies = await _context.Movies
                .Include(m => m.Genre)
                .Where(i => i.Actors!.Any(i => i.ActorID == id))
                .OrderBy(i => i.Title)
                .ToListAsync();
            var output = new ActorMoviesVm
            {
                ActorDetail = _mapper.Map<CardActorVm>(Actor),
                Movies = _mapper.Map<List<CardMovieVm>>(Movies)
            };
            return View(output);
        }

        [Route("Actor/ContactInfo/{id}")]
        public async Task<IActionResult> ContactInfo(int id)
        {
            /* var Actors = await _context.Actors
                 .Include(t => t.Movies!)
                     .ThenInclude(t => t.Movie)
                 .Include(t => t.ContactInfo)
                 .OrderBy(i => i.LastName)
                 .ThenBy(i => i.FirstName)
                 .ToListAsync();
             return View(_mapper.Map<List<CardActorVm>>(Actors));*/
            if (id == null || _context.Actors == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var output = _mapper.Map<ContactInfoVm>(actor);

            var ContactInfo = await _context.ContactInfos.FindAsync(id);
            if (ContactInfo is not null)
            {
                output.Email = ContactInfo.Email;
                output.AgentPhoneNumber = ContactInfo.AgentPhoneNumber;
            }
            return View(output);
        }
    }
}
