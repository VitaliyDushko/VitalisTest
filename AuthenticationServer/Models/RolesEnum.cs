using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models
{
    public enum RolesEnum
    {
        Admin,
        [Display(Name = "Regular User")]
        RegularUser
    }
}
