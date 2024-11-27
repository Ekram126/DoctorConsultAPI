using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  DoctorConsult.ViewModels.DoctorVM
{
    public class SortDoctorVM
    {

        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? NameAr { get; set; }
        public string? NationalId { get; set; }

        public string? Mobile { get; set; }
        public DateTime? Dob { get; set; }

        public string? Email { get; set; }
        public string? Address { get; set; }

        public string? AddressAr { get; set; }
        public DateTime? GradDate { get; set; }

        public DateTime? JoinDate { get; set; }

        public string? Remarks { get; set; }
        public int? GenderId { get; set; }

        public int? SpecialistId { get; set; }
        public string? SortStatus { get; set; }

     public string? SortBy { get; set; }
    }
}
