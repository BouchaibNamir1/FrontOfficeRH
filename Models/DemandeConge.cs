using System;
using System.ComponentModel.DataAnnotations;

namespace FrontOfficeRH1.Models
{
    public class DemandeConge
    {
        public int Id { get; set; }

        [Required]
        public DateTime DateDebut { get; set; }

        [Required]
        public DateTime DateFin { get; set; }

        public string Statut { get; set; } = null!; // EnAttente, Validée, Refusée

        public int EmployeId { get; set; }
        public Employe Employe { get; set; } = null!;
    }
}
