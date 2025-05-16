using Microsoft.EntityFrameworkCore;

namespace FrontOfficeRH1.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Candidat> Candidats { get; set; }
        public DbSet<Employe> Employes { get; set; }
        public DbSet<OffreEmploi> OffresEmploi { get; set; }
        public DbSet<Candidature> Candidatures { get; set; }
        public DbSet<DemandeConge> DemandesConges { get; set; }
        public DbSet<DemandeAttestation> DemandesAttestations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ajoute ici des configurations spécifiques si besoin
        }
    }
}
