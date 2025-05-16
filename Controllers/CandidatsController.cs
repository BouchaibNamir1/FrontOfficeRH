using FrontOfficeRH1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrontOfficeRH1.Controllers
{
    [Authorize(Roles = "Candidat")]
    public class CandidatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CandidatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Redirection vers la liste des offres
        public IActionResult Index()
        {
            return View(); // charge Views/Candidats/Index.cshtml
        }

        // Liste des offres disponibles
        public async Task<IActionResult> Offres()
        {
            var offres = await _context.OffresEmploi.ToListAsync();
            return View(offres);
        }

        // Action de postuler à une offre
        public async Task<IActionResult> Postuler(int id)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            var utilisateur = await _context.Utilisateurs
                .Include(u => u.Candidat)
                .FirstOrDefaultAsync(u => u.NomUtilisateur == username);

            if (utilisateur == null || utilisateur.Candidat == null)
                return NotFound();

            // Vérifie si le candidat a déjà postulé
            var dejaPostule = await _context.Candidatures.AnyAsync(c =>
                c.CandidatId == utilisateur.Candidat.Id && c.OffreEmploiId == id);

            if (dejaPostule)
                return RedirectToAction("Offres");

            var candidature = new Candidature
            {
                CandidatId = utilisateur.Candidat.Id,
                OffreEmploiId = id,
                Statut = "EnAttente",
                DateCandidature = DateTime.Now
            };

            _context.Candidatures.Add(candidature);
            await _context.SaveChangesAsync();

            return RedirectToAction("MesCandidatures");
        }

        // Affiche les candidatures du candidat connecté
        public async Task<IActionResult> MesCandidatures()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            var utilisateur = await _context.Utilisateurs
                .Include(u => u.Candidat)
                .ThenInclude(c => c.Candidatures)
                .ThenInclude(c => c.OffreEmploi)
                .FirstOrDefaultAsync(u => u.NomUtilisateur == username);

            if (utilisateur?.Candidat == null)
                return NotFound();

            var candidatures = utilisateur.Candidat.Candidatures ?? new List<Candidature>();
            return View(candidatures);
        }

        // Permet d’annuler une candidature en attente
        public async Task<IActionResult> Annuler(int id)
        {
            var candidature = await _context.Candidatures.FindAsync(id);

            if (candidature != null && candidature.Statut == "EnAttente")
            {
                _context.Candidatures.Remove(candidature);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("MesCandidatures");
        }
    }
}
