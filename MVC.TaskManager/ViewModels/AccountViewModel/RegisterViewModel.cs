using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC.TaskManager.ViewModels.AccountViewModel
{
    public class RegisterViewModel
    {
        public Guid Id { get; set; }

        [EmailAddress]
        [DisplayName("Email")]
        public string UserName { get; set; }
        public string CompleteName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string SelectedRole { get; set; }
        public string Image { get; set; }
    }
}
