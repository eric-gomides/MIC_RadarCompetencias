using System.ComponentModel.DataAnnotations;

namespace RadarCompetencias.Models.UsersViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email obrigatório.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha obrigatória.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

    }
}
