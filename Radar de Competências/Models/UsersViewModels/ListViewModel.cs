using System.ComponentModel.DataAnnotations;

namespace RadarCompetencias.Models.UsersViewModels
{
    public class ListViewModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }
        public string Email { get; set; }
        [Display(Name = "Nível de acesso")]
        public string Role { get; set; }
        [Display(Name = "Nome do usuário")]
        public string UserName { get; set; }
    }
}
