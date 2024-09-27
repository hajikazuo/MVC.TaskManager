using System.ComponentModel.DataAnnotations;

namespace MVC.TaskManager.ViewModels
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string SelectedRole { get; set; }
    }
}
