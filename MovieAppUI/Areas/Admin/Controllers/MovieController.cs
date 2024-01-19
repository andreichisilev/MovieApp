using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieAppUI.Areas.Admin.Models.DTOs;
using MovieAppUI.Areas.Admin.Models.ViewModels;
using MovieAppUI.Data;
using MovieAppUI.Models.Entities;

namespace MovieAppUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]

    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MovieController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Admin/Movie
        [Route("Admin/Movies")]
        public async Task<IActionResult> Index()
        {
            if (_context.Actors == null)
                return Problem("Entity set 'ApplicationDbContext.Actors'  is null.");
            var movies = _mapper.Map<List<MovieVm>>(
                await _context.Movies
                    .Include(m => m.Genre)
                    .ToListAsync());
            return View(movies);
        }

        // GET: Admin/Movie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            var ActorList = await _context.MovieActors
                .Include(ma => ma.Actor)
                .Where(m => m.MovieID == id)
                .OrderBy(o => o.Actor!.LastName)
                .ThenBy(o => o.Actor!.FirstName)
                .ToListAsync();

            var output = _mapper.Map<MovieDetailsVm>(movie);
            var actors = "";

            foreach(var actor in ActorList)
            {
                actors += $"{actor.Actor!.FirstName} {actor.Actor!.LastName}({actor.Role}), ";
            }
            output.Actors = string.IsNullOrEmpty(actors) ? "-" : actors.Substring(0, actors.Length - 2);
            return View(output);
        }

        // GET: Admin/Movie/Create
        public IActionResult Create()
        {
            ViewData["GenreID"] = new SelectList(_context.Genres, "Id", "Name");
            return View();
        }

        // POST: Admin/Movie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] NewMovieDto newMovie)
        {
            if (ModelState.IsValid)
            {
                var NewMovie = _mapper.Map<Movie>(newMovie);
                _context.Add(NewMovie);
                await _context.SaveChangesAsync();
                TempData["MovieMessage"] = $"Movie <strong>{NewMovie.Title}</strong> has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreID"] = new SelectList(_context.Genres, "Id", "Name", newMovie.GenreID);
            return View(newMovie);
        }

        // GET: Admin/Movie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var oldMovie = await _context.Movies.FindAsync(id);
            if (oldMovie == null)
            {
                return NotFound();
            }
            ViewData["GenreID"] = new SelectList(_context.Genres, "Id", "Name", oldMovie.GenreID);
            var actors = await _context.MovieActors
                .Include(ma => ma.Actor).Where(m => m.MovieID == id)
                .OrderBy(o => o.Actor.LastName)
                .ThenBy(o => o.Actor.FirstName)
                .ToListAsync();
            var output = new MovieEditDto()
            {
                OldMovie = _mapper.Map<OldMovieDto>(oldMovie),
                ActorList = _mapper.Map<List<ActorVm>>(actors)
            };
            return View(output);
        }

        // POST: Admin/Movie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, [FromForm] MovieEditDto UpdatedMovie)
        {
            if (UpdatedMovie.OldMovie is null || id != UpdatedMovie.OldMovie!.MovieID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var OldMovie = await _context.Movies.FindAsync(id);
                    if (OldMovie is null)
                    {
                        return NotFound();
                    }
                    TempData["MovieMessage"] = $"Movie <strong>{OldMovie.Title}</strong> was updated to <strong>{UpdatedMovie.OldMovie.Title}</strong> successfully!";
                    UpdatedMovie.OldMovie.ToEntity(ref OldMovie);
                    _context.Update(OldMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(UpdatedMovie.OldMovie.MovieID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreID"] = new SelectList(_context.Genres, "Id", "Name", UpdatedMovie.OldMovie.GenreID);
            var actors = await _context.MovieActors
                .Include(ma => ma.Actor)
                .FirstOrDefaultAsync(m => m.MovieID == id);
            UpdatedMovie.ActorList = _mapper.Map<List<ActorVm>>(actors);
            return View(UpdatedMovie);
        }

        // GET: Admin/Movie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            var ActorList = await _context.MovieActors
                .Include(ma => ma.Actor)
                .Where(m => m.MovieID == id)
                .OrderBy(o => o.Actor!.LastName)
                .ThenBy(o => o.Actor!.FirstName)
                .ToListAsync();
            var output = _mapper.Map<MovieDetailsVm>(movie);
            var actors = "";

            foreach (var actor in ActorList)
            {
                actors += $"{actor.Actor!.FirstName} {actor.Actor!.LastName}({actor.Role}), ";
            }
            output.Actors = string.IsNullOrEmpty(actors) ? "-" : actors.Substring(0, actors.Length - 2);
            return View(output);
        }

        // POST: Admin/Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movies == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Movies'  is null.");
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //add actor to movie
        public async Task<IActionResult> AddActor(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = _mapper.Map<AddActorToMovieDto>(await _context.Movies.FindAsync(id));
            if (movie == null)
            {
                return NotFound();
            }

            var actors = _mapper.Map<List<ActorDto>>(
                await _context.Actors
                    .Include(t => t.Movies)
                    .ToListAsync());
            ViewBag.AvailableActors = new SelectList(actors, "ActorID", "Name");



            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddActor(int id, [FromForm] AddActorToMovieDto AddActorToMovie)
        {
            var movie = await _context.Movies.FindAsync(id);
            var actor = await _context.Actors.FindAsync(AddActorToMovie.ActorID);
            if (movie is null || actor is null)
                return NotFound();
            if (id != AddActorToMovie.MovieID || id != movie!.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var movieActor = await _context.MovieActors.FirstOrDefaultAsync(q => q.MovieID == AddActorToMovie.MovieID && q.ActorID == AddActorToMovie.ActorID);
                    if (movieActor is null)
                    {
                        MovieActor MovieActor = new()
                        {
                            MovieID = AddActorToMovie.MovieID,
                            ActorID = AddActorToMovie.ActorID,
                            Role = AddActorToMovie.Role
                        };
                        await _context.MovieActors.AddAsync(MovieActor);
                        await _context.SaveChangesAsync();
                        TempData["MovieMessage"] = $"Actor <strong>{actor.FirstName} {actor.LastName}</strong> was successfully assigned to movie {movie.Title}!";
                    }
                    else
                    {
                        TempData["MovieMessage"] = $"Actor <strong> {actor.FirstName} {actor.LastName}</strong> is already assigned to movie {movie.Title}!";
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieActorExists(AddActorToMovie.MovieID, AddActorToMovie.ActorID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(AddActorToMovie);
        }   

        private bool MovieExists(int id)
        {
          return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool MovieActorExists(int MovieID, int ActorID)
        {
            return (_context.MovieActors?.Any(e => e.MovieID == MovieID && e.ActorID == ActorID)).GetValueOrDefault();
        }
    }
}
