using System.ComponentModel.DataAnnotations;

namespace RadarCompetencias.Models.UsersViewModels
{
    public class DeleteViewModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }       
        public int Id { get; set; }
    }
}
