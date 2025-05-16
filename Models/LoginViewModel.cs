using System.ComponentModel.DataAnnotations;

namespace FrontOfficeRH1.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Nom d'utilisateur")]
        public string NomUtilisateur { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string MotDePasse { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
