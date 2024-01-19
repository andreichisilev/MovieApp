using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieAppUI.Areas.Admin.Models.DTOs;
using MovieAppUI.Data;
using MovieAppUI.Models.Entities;
using MovieAppUI.Areas.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace MovieAppUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class ActorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ActorController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Admin/Actor
        [Route("Admin/Actors")]
        public async Task<IActionResult> Index()
        {
            /*return _context.Actors != null ? 
                        View(await _context.Actors.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Actors'  is null.");*/
            if (_context.Actors == null)
                return Problem("Entity set 'ApplicationDbContext.Actors'  is null.");
            var actors = await _context.Actors.ToListAsync();
            return View(_mapper.Map<List<ActorVm>>(actors));

        }

        // GET: Admin/Actor/Details/5
        [Route("Admin/Actor/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Actors == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ActorDetailsVm>(actor));
        }

        // GET: Admin/Actor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Actor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] NewActorDto newActor)
        {
            if (ModelState.IsValid)
            {
                var actor = _mapper.Map<Actor>(newActor);
                _context.Add(actor);
                await _context.SaveChangesAsync();
                TempData["ActorMessage"] = $"Actor <strong>{actor.FirstName} {actor.LastName}</strong> has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(newActor);
        }

        // GET: Admin/Actor/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (_context.Actors == null)
            {
                return NotFound();
            }

            var oldActor = _mapper.Map<OldActorDto>(await _context.Actors.FindAsync(id));
            if (oldActor == null)
            {
                return NotFound();
            }
            return View(oldActor);
        }

        // POST: Admin/Actor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] OldActorDto oldActor)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (id != oldActor.ActorID || id != actor!.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["ActorMessage"] = $"Actor <strong>{actor.FirstName} {actor.LastName}</strong> was updated to <strong>{oldActor.FirstName} {oldActor.LastName}</strong> succesfully!";
                    oldActor.ToEntity(ref actor);
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(oldActor.ActorID))
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
            return View(oldActor);
        }

            // GET: Admin/Actor/Delete/5
            public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Actors == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ActorVm>(actor));
        }

        // POST: Admin/Actor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Actors == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Actors'  is null.");
            }
            var OldActor = await _context.Actors.FindAsync(id);
            if (OldActor is null)
                return NotFound();
            var ActorInMovies = await _context.MovieActors.Where(q => q.ActorID == id).CountAsync();
            if (ActorInMovies == 0)
            {
                _context.Actors.Remove(OldActor);
                TempData["ActorMessage"] = $"The actor <strong>{OldActor.FirstName} {OldActor.LastName}</strong> was deleted successfully!";
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["ActorMessage"] = $"<strong>{OldActor.FirstName} {OldActor.LastName}</strong> can NOT be deleted because he plays in {ActorInMovies} movies!";
            }
            return RedirectToAction(nameof(Index));
        }
        [Route("Admin/Actors/ContactInfo/{id}")]
        public async Task<IActionResult> ContactInfo(int? id)
        {
            if (id == null || _context.Actors == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var output = _mapper.Map<ContactInfoDto>(actor);
            
            var ContactInfo = await _context.ContactInfos.FindAsync(id);
            if (ContactInfo is not null)
            {
                output.Email = ContactInfo.Email;
                output.AgentPhoneNumber = ContactInfo.AgentPhoneNumber;
                output.IsNew = false;
            }
            return View(output);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactInfo([FromRoute] int? id, [FromForm] ContactInfoDto NewContactInfo)
        {
            if (id == null || _context.Actors == null)
            {
                return NotFound();
            }
            var OldActor = await _context.Actors.FindAsync(id);
            if (OldActor == null)
            {
                return RedirectToAction(nameof(Index));
            }
            if (ModelState.IsValid)
            {
                var OldContactInfo = await _context.ContactInfos
                    .Include(t => t.Actor)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (OldContactInfo is null)
                {
                    // add ContactInfo
                    var ContactInfo = _mapper.Map<ContactInfo>(NewContactInfo);
                    await _context.AddAsync(ContactInfo);
                }
                else
                {
                    // update ContactInfo
                    OldContactInfo.Email = NewContactInfo.Email;
                    OldContactInfo.AgentPhoneNumber = NewContactInfo.AgentPhoneNumber;
                    _context.Update(OldContactInfo);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(NewContactInfo);
        }

        private bool ActorExists(int id)
        {
          return (_context.Actors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
