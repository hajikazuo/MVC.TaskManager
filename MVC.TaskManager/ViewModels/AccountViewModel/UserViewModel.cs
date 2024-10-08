﻿using System.ComponentModel.DataAnnotations;

namespace MVC.TaskManager.ViewModels.AccountViewModel
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string CompleteName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Image { get; set; }
    }
}
