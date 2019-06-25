using System.ComponentModel.DataAnnotations;

namespace RadarCompetencias.Models.UsersViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nome completo obrigatório.")]
        [Display(Name = "Nome Completo")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Nome de usuário obrigatório.")]
        [StringLength(40, ErrorMessage = "Nome do usuário não pode ser maior que 40 caracteres."),
        RegularExpression(@"[^\s*].*[^\s*]", ErrorMessage = "Nome de usuário inválido.")]
        [Display(Name = "Nome de usuário")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha obrigatória.")]
        [StringLength(100, ErrorMessage = "A senha deve conter no mínimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Digite a senha novamente")]
        [Compare("Password", ErrorMessage = "Senhas diferentes.")]
        public string ConfirmPassword { get; set; }
    }
}
