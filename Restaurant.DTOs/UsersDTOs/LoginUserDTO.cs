using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DTOs.UsersDTOs
{
    public class LoginUserDTO
    {
        [Required]
        [MinLength(3)]
        public string UserName { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMy { get; set; } = false;
    }
}
