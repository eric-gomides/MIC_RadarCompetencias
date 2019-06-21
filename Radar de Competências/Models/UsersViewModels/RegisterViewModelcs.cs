using System.ComponentModel.DataAnnotations;

namespace RadarCompetencias.Models.UsersViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Nome Completo")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Nome de usuário")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Digite a senha novamente")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
