using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaySystemsAPI.DTOs
{
    public class UserRecoveryAccountDTO
    {
        [Required]
        public string Email { get; set; }
    }
}
