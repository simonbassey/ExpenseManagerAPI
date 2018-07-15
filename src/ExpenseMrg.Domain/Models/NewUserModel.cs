using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseMgr.Domain.Models
{
    public class NewUserModel
    {

        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
