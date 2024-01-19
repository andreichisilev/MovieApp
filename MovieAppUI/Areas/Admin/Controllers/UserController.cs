using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieAppUI.Areas.Admin.Models.DTOs;
using MovieAppUI.Data;
using MovieAppUI.Models.CustomIdentity;

namespace MovieAppUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]

    public class UserController : Controller
    {
        public readonly IMapper _mapper;
        public readonly ApplicationDbContext _context;
        public readonly SignInManager<AppUser> _signInManager;
        public readonly UserManager<AppUser> _userManager;
        public readonly RoleManager<AppRole> _roleManager;

        public UserController(IMapper mapper, ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        // GET: Admin/User
        public async Task<IActionResult> Index()
        {
            var users = _mapper.Map<List<ExistingUserDto>>(await _userManager.Users.ToListAsync());
            foreach (var user in users)
            {
                var currentUser = await _userManager.FindByEmailAsync(user.Email);
                user.IsAdmin = await _userManager.IsInRoleAsync(currentUser!, "Administrator");
            }
            return View(users);
        }



        public async Task<IActionResult> Edit(int id)
        {
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == id);
            if (existingUser is null)
                return NotFound();
            var output = _mapper.Map<ExistingUserDto>(existingUser);
            output.IsAdmin = await _userManager.IsInRoleAsync(existingUser, "Administrator");
            return View(output);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] ExistingUserDto updatedUser)
        {
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == id);
            if (existingUser is null)
                return NotFound();
            if (id != updatedUser.Id || !updatedUser.Email.Equals(existingUser.Email))
                return BadRequest();
            if (ModelState.IsValid)
            {

                StringBuilder mesaj = new();

                if (updatedUser.IsAdmin)
                {
                    if (!await _userManager.IsInRoleAsync(existingUser, "Administrator"))
                    {
                        await _userManager.AddToRoleAsync(existingUser, "Administrator");
                        mesaj.Append($"User <strong>{existingUser.Email}</strong> is now an <strong>Administrator</strong>!<br/>");
                    }
                    else
                    {
                        mesaj.Append($"User <strong>{existingUser.Email}</strong> is still an <strong>Administrator</strong>!");
                    }
                }
                else
                {
                    if (await _userManager.IsInRoleAsync(existingUser, "Administrator"))
                    {
                        await _userManager.RemoveFromRolesAsync(existingUser, new string[] { "Administrator" });
                        mesaj.Append($"User <strong>{existingUser.Email}</strong> is not an <strong>Administrator</strong> anymore!<br/>");
                    }
                    else
                    {
                        mesaj.Append($"User <strong>{existingUser.Email}</strong> is still not an <strong>Administrator</strong>!");

                    }
                }
                TempData["UserMessage"] = mesaj.ToString();

                var currentUser = _userManager.Users.FirstOrDefault(u => u.Email == User.Identity!.Name);
                if (currentUser != null && currentUser!.Id == existingUser.Id)
                {
                    await _signInManager.RefreshSignInAsync(currentUser!);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(updatedUser);
        }
    }
}