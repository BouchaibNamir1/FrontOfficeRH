using System.ComponentModel.DataAnnotations;

namespace FrontOfficeRH1.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }

        [Required]
        public string NomUtilisateur { get; set; } = null!;

        [Required]
        public string MotDePasse { get; set; } = null!;

        [Required]
        public string Role { get; set; } = null!; // "Candidat" ou "Employe"

        public Candidat? Candidat { get; set; }
        public Employe? Employe { get; set; }
    }
}
