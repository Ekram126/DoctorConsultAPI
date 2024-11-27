using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.ViewModels.DoctorVM
{
    public class CreateDoctorVM
    {
        public int Id { get; set; }


        public string? Code { get; set; }

        public string? Name { get; set; }


        public string? NameAr { get; set; }


        public string? NationalId { get; set; }

        public string? Mobile { get; set; }

        public DateTime? Dob { get; set; }

        public string? StrDob { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        public string? AddressAr { get; set; }
        public DateTime? GradDate { get; set; }
        public string? StrGradDate { get; set; }
        public DateTime? JoinDate { get; set; }

        public string? StrJoinDate { get; set; }

        public string? Remarks { get; set; }
        public int? GenderId { get; set; }

        public int? SpecialistId { get; set; }
        public int? ParentId { get; set; }
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }

        public bool? IsActive { get; set; }

        public string? DoctorImg { get; set; }

    }
}
