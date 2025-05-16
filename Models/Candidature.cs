namespace FrontOfficeRH1.Models
{
    public class Candidature
    {
        public int Id { get; set; }

        public int CandidatId { get; set; }
        public Candidat Candidat { get; set; } = null!;

        public int OffreEmploiId { get; set; }
        public OffreEmploi OffreEmploi { get; set; } = null!;

        public string Statut { get; set; } = null!;
        // EnAttente, Acceptée, Refusée
        public DateTime DateCandidature { get; set; } = DateTime.Now;

    }
}
