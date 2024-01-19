using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieAppUI.Areas.Admin.Models.DTOs;
using MovieAppUI.Data;
using MovieAppUI.Models.CustomIdentity;

namespace MovieAppUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]

    public class RoleController : Controller
    {
        public readonly IMapper _mapper;
        public readonly ApplicationDbContext _context;
        public readonly UserManager<AppUser> _userManager;
        public readonly RoleManager<AppRole> _roleManager;

        public RoleController(IMapper mapper, ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin/Role
        public async Task<IActionResult> Index()
        {
            var roles = _mapper.Map<List<ExistingRoleDto>>(await _roleManager.Roles.ToListAsync());
            return View(roles);
        }

        // GET: Admin/Role/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Role/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] NewRoleDto newRole)
        {
            if (ModelState.IsValid)
            {
                if (!await _roleManager.RoleExistsAsync(newRole.Name))
                {
                    await _roleManager.CreateAsync(new AppRole() { Name = newRole.Name});
                    TempData["RoleMessage"] = $"Role <strong>{newRole.Name}</strong> has been added successfully!";
                }
                else
                {
                    TempData["RoleMessage"] = $"Role <strong>{newRole.Name}</strong> already exists!";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(newRole);
        }

        // GET: Admin/Role/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var existingRole = _mapper.Map<ExistingRoleDto>(await _roleManager.Roles.FirstOrDefaultAsync(i => i.Id == id));
            if (existingRole == null)
                return NotFound();
            return View(existingRole);
        }

        // POST: Admin/Role/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] ExistingRoleDto updatedRole)
        {
            var existingRole = await _roleManager.Roles.FirstOrDefaultAsync(i => i.Id == id);
            if (existingRole is null)
                return NotFound();
            if (existingRole.Id != id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                if (!await _roleManager.RoleExistsAsync(updatedRole.Name))
                {
                    existingRole.Name = updatedRole.Name;
                    await _roleManager.UpdateAsync(existingRole);
                    TempData["MesajRol"] = $"Role <strong>{updatedRole.Name}</strong> was updated successfully!";
                }
                else
                {
                    TempData["MesajRol"] = $"Role <strong>{updatedRole.Name}</strong> already exists!";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(updatedRole);
        }

        // GET: Admin/Role/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var existingRole = _mapper.Map<ExistingRoleDto>(await _roleManager.Roles.FirstOrDefaultAsync(i => i.Id == id));
            if (existingRole == null)
                return NotFound();
            return View(existingRole);
        }

        // POST: Admin/Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var existingRole = await _roleManager.Roles.FirstOrDefaultAsync(i => i.Id == id);
            if (existingRole == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                var utilizatori = await _userManager.GetUsersInRoleAsync(existingRole.Name!);
                if (utilizatori.Count > 0)
                {
                    TempData["MesajRol"] = $"Role <strong>{existingRole.Name}</strong> can not be deleted because it has {utilizatori.Count} users!";
                }
                else
                {
                    await _roleManager.DeleteAsync(existingRole);
                    TempData["MesajRol"] = $"Role <strong>{existingRole.Name}</strong> has been deleted!";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(_mapper.Map<ExistingRoleDto>(existingRole));
        }

        
    }
}
