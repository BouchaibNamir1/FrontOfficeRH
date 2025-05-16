using System.ComponentModel.DataAnnotations;

namespace FrontOfficeRH1.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string NomUtilisateur { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string MotDePasse { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
