using System.ComponentModel.DataAnnotations;

namespace Radar_de_Competências.Models.UsersViewModels
{
    public class EditViewModel
    {

        [Display(Name = "Name")]
        public string Name { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
