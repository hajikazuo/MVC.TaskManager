using System.ComponentModel.DataAnnotations;

namespace MVC.TaskManager.ViewModels.AccountViewModel
{
    public class ChangePasswordViewModel
    {
        public Guid Id { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
