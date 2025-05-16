namespace FrontOfficeRH1.Models
{
    public class DemandeAttestation
    {
        public int Id { get; set; }

        public string TypeAttestation { get; set; } = null!; // Travail, Salaire, etc.
        public string Statut { get; set; } = null!;

        public int EmployeId { get; set; }
        public Employe Employe { get; set; } = null!;
    }
}
