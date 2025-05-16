// -------------------- EmployesController.cs --------------------
using FrontOfficeRH1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrontOfficeRH1.Controllers
{
    [Authorize(Roles = "Employe")]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Redirection vers la page d'accueil de l'espace employé
        public IActionResult Index()
        {
            return View(); // Views/Employes/Index.cshtml doit exister
        }

        public IActionResult DemandeConge()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DemandeConge(DateTime dateDebut, DateTime dateFin)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var utilisateur = await _context.Utilisateurs
                .Include(u => u.Employe)
                .FirstOrDefaultAsync(u => u.NomUtilisateur == username);

            if (utilisateur?.Employe == null) return NotFound();

            var demande = new DemandeConge
            {
                DateDebut = dateDebut,
                DateFin = dateFin,
                Statut = "EnAttente",
                EmployeId = utilisateur.Employe.Id
            };

            _context.DemandesConges.Add(demande);
            await _context.SaveChangesAsync();
            return RedirectToAction("HistoriqueConges");
        }

        public IActionResult DemandeAttestation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DemandeAttestation(string typeAttestation)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var utilisateur = await _context.Utilisateurs
                .Include(u => u.Employe)
                .FirstOrDefaultAsync(u => u.NomUtilisateur == username);

            if (utilisateur?.Employe == null) return NotFound();

            var demande = new DemandeAttestation
            {
                TypeAttestation = typeAttestation,
                Statut = "EnAttente",
                EmployeId = utilisateur.Employe.Id
            };

            _context.DemandesAttestations.Add(demande);
            await _context.SaveChangesAsync();
            return RedirectToAction("HistoriqueAttestations");
        }

        public async Task<IActionResult> HistoriqueConges()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var utilisateur = await _context.Utilisateurs
                .Include(u => u.Employe)
                .ThenInclude(e => e.DemandesConges)
                .FirstOrDefaultAsync(u => u.NomUtilisateur == username);

            if (utilisateur?.Employe == null) return NotFound();

            return View(utilisateur.Employe.DemandesConges ?? new List<DemandeConge>());
        }

        public async Task<IActionResult> HistoriqueAttestations()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var utilisateur = await _context.Utilisateurs
                .Include(u => u.Employe)
                .ThenInclude(e => e.DemandesAttestations)
                .FirstOrDefaultAsync(u => u.NomUtilisateur == username);

            if (utilisateur?.Employe == null) return NotFound();

            return View(utilisateur.Employe.DemandesAttestations ?? new List<DemandeAttestation>());
        }

        public async Task<IActionResult> AnnulerConge(int id)
        {
            var demande = await _context.DemandesConges.FindAsync(id);
            if (demande != null && demande.Statut == "EnAttente")
            {
                _context.DemandesConges.Remove(demande);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("HistoriqueConges");
        }

        public async Task<IActionResult> AnnulerAttestation(int id)
        {
            var demande = await _context.DemandesAttestations.FindAsync(id);
            if (demande != null && demande.Statut == "EnAttente")
            {
                _context.DemandesAttestations.Remove(demande);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("HistoriqueAttestations");
        }
    }
}
