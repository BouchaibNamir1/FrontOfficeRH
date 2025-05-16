using FrontOfficeRH1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FrontOfficeRH1.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.NomUtilisateur == model.NomUtilisateur);

            if (user != null && user.MotDePasse == model.MotDePasse)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.NomUtilisateur),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return user.Role == "Employe" ? RedirectToAction("Index", "Employees") : RedirectToAction("Index", "Candidats");
            }

            ModelState.AddModelError("", "Nom d'utilisateur ou mot de passe incorrect.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new Utilisateur
            {
                NomUtilisateur = model.NomUtilisateur,
                MotDePasse = model.MotDePasse,
                Role = model.Role
            };

            _context.Utilisateurs.Add(user);
            await _context.SaveChangesAsync();

            if (model.Role == "Employe")
            {
                _context.Employes.Add(new Employe
                {
                    UtilisateurId = user.Id,
                    NomComplet = model.NomUtilisateur
                });
            }
            else if (model.Role == "Candidat")
            {
                _context.Candidats.Add(new Candidat
                {
                    UtilisateurId = user.Id,
                    NomComplet = model.NomUtilisateur
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
