using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FrontOfficeRH1.Models
{
    public class OffreEmploi
    {
        public int Id { get; set; }

        [Required]
        public string Titre { get; set; } = null!;

        public string Description { get; set; } = null!;

        public List<Candidature> Candidatures { get; set; } = new();
    }
}
