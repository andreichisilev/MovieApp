using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieAppUI.Areas.Admin.Models.DTOs;
using MovieAppUI.Data;
using MovieAppUI.Models.Entities;

namespace MovieAppUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]

    public class GenreController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GenreController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Admin/Genre
        [Route("Admin/Genres")]
        public async Task<IActionResult> Index()
        {
            TempData["Title"] = "Genre page";
            if (_context.Genres == null)
                return Problem("Entity set 'ApplicationDbContext.Monezi'  is null.");
            return View(_mapper.Map<List<OldGenreDto>>(await _context.Genres.ToListAsync()));
        }

        // GET: Admin/Genre/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<OldGenreDto>(genre));
        }

        // GET: Admin/Genre/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Genre/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] NewGenreDto newGenre)
        {
            if (ModelState.IsValid)
            {
                var genre = _mapper.Map<Genre>(newGenre);
                _context.Add(genre);
                await _context.SaveChangesAsync();
                TempData["GenreMessage"] = $"Genre <strong>{genre.Name}</strong> was added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(newGenre);
        }

        // GET: Admin/Genre/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var oldGenre = await _context.Genres.FindAsync(id);
            if (oldGenre == null)
            {
                return NotFound();
            }
            return View(_mapper.Map<OldGenreDto>(oldGenre));
        }

        // POST: Admin/Genre/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] OldGenreDto updatedGenre)
        {
            if (id != updatedGenre.GenreID)
            {
                return NotFound();
            }
            var oldGenre = await _context.Genres.FindAsync(id);
            if (oldGenre is null)
            {
                return NotFound();
            }
            if( id != updatedGenre.GenreID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["GenreMessage"] = $"Genre <strong>{oldGenre.Name}</strong> was renamed to <strong>{updatedGenre.Name}</strong> successfully!";
                    oldGenre.Name = updatedGenre.Name;
                    _context.Update(oldGenre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(oldGenre.Id))
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
            return View(updatedGenre);
        }

        // GET: Admin/Genre/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var oldGenre = _mapper.Map<OldGenreDto>(await _context.Genres.FirstOrDefaultAsync(m => m.Id == id));
            if (oldGenre == null)
            {
                return NotFound();
            }

            return View(oldGenre);
        }

        // POST: Admin/Genre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Genres == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Genres'  is null.");
            }
            var oldGenre = await _context.Genres
                .Include(t => t.Movies)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oldGenre == null)
            {
                return NotFound();
            }
            else
            {
                if(oldGenre.Movies!.Count() > 0)
                {
                    TempData["GenreMessage"] = $"Genre <strong>{oldGenre.Name}</strong> cannot be deleted because it is used by one or more movies!";
                }
                else
                {
                    _context.Genres.Remove(oldGenre);
                    TempData["GenreMessage"] = $"Genre <strong>{oldGenre.Name}</strong> was deleted successfully!";
                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenreExists(int id)
        {
          return (_context.Genres?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
