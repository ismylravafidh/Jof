using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Jof.ViewModels
{
    public class RegisterVm
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}
