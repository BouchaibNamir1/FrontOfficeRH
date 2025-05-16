using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FrontOfficeRH1.Models
{
    public class Candidat
    {
        public int Id { get; set; }

        [Required]
        public string NomComplet { get; set; } = null!;

        public int UtilisateurId { get; set; }
        public Utilisateur Utilisateur { get; set; } = null!;

        public List<Candidature> Candidatures { get; set; } = new();
    }
}
