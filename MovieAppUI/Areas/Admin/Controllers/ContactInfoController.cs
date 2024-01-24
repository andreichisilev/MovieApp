using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieAppUI.Data;
using MovieAppUI.Models.Entities;

namespace MovieAppUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ContactInfo
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ContactInfos.Include(c => c.Actor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/ContactInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ContactInfos == null)
            {
                return NotFound();
            }

            var contactInfo = await _context.ContactInfos
                .Include(c => c.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactInfo == null)
            {
                return NotFound();
            }

            return View(contactInfo);
        }

        // GET: Admin/ContactInfo/Create
        public IActionResult Create()
        {
            //Create select list for actors without actors that already have contact info
            var actors = _context.Actors.Where(a => a.ContactInfo == null);
            ViewData["Id"] = new SelectList(actors, "Id", "LastName");
            return View();
        }

        // POST: Admin/ContactInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,AgentPhoneNumber")] ContactInfo contactInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Actors, "Id", "Id", contactInfo.Id);
            return View(contactInfo);
        }

        // GET: Admin/ContactInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ContactInfos == null)
            {
                return NotFound();
            }

            var contactInfo = await _context.ContactInfos.FindAsync(id);
            if (contactInfo == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Actors, "Id", "Id", contactInfo.Id);
            return View(contactInfo);
        }

        // POST: Admin/ContactInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,AgentPhoneNumber")] ContactInfo contactInfo)
        {
            if (id != contactInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactInfoExists(contactInfo.Id))
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
            ViewData["Id"] = new SelectList(_context.Actors, "Id", "Id", contactInfo.Id);
            return View(contactInfo);
        }

        // GET: Admin/ContactInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ContactInfos == null)
            {
                return NotFound();
            }

            var contactInfo = await _context.ContactInfos
                .Include(c => c.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactInfo == null)
            {
                return NotFound();
            }

            return View(contactInfo);
        }

        // POST: Admin/ContactInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ContactInfos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ContactInfos'  is null.");
            }
            var contactInfo = await _context.ContactInfos.FindAsync(id);
            if (contactInfo != null)
            {
                _context.ContactInfos.Remove(contactInfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactInfoExists(int id)
        {
          return (_context.ContactInfos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
