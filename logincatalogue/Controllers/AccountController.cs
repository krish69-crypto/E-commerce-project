using logincatalogue.Data;
using logincatalogue.Models;
using logincatalogue.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logincatalogue.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDBContext _context;
        public AccountController(AppDBContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(a =>
                    a.Email == model.Email &&
                    a.password == model.Password);

            if (account == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid email or password.");

                return View(model);
            }

            HttpContext.Session.SetInt32("Id", account.ID);
            HttpContext.Session.SetString("Name", account.name);

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(account account)
        {
            if (!ModelState.IsValid)
            {
                return View(account);
            }

            bool emailExists = await _context.Account
                .AnyAsync(a => a.Email == account.Email);

            if (emailExists)
            {
                ModelState.AddModelError(
                    "Email",
                    "Email is already registered.");

                return View(account);
            }

            _context.Account.Add(account);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Registration successful. Please login.";

            return RedirectToAction("Login");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Remove the logged-in user's ID from the session
             HttpContext.Session.Remove("Id");

            // Clear all session data
            HttpContext.Session.Clear();

            // Redirect to Login page
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
           

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Account.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "The entered email address does not exist.");
                return View(model);
            }


            user.password = model.NewPassword;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Password changed successfully!";
            return RedirectToAction("ChangePassword");
        }
    }
}