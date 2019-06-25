using System.ComponentModel.DataAnnotations;

namespace RadarCompetencias.Models.UsersViewModels
{
    public class EditViewModel
    {

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string Id { get; set; }
    }
}
