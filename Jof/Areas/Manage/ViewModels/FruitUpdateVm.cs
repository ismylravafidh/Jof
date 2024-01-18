using System.ComponentModel.DataAnnotations;

namespace Jof.Areas.Manage.ViewModels
{
    public class FruitUpdateVm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string? ImgUrl { get; set; }
        public IFormFile Image { get; set; }
    }
}
