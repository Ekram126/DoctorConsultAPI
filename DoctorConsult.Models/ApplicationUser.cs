using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorConsult.Models
{
    public class ApplicationUser : IdentityUser
    {


        [NotMapped]
        public List<string>? RoleNames { get; set; }


    }
}
