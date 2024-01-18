using System.ComponentModel.DataAnnotations;

namespace Jof.Areas.Manage.ViewModels
{
    public class FruitCreateVm
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
