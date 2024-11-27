using System;

namespace  DoctorConsult.ViewModels.DoctorVM
{
    public class EditDoctorVM
    {
        public int Id { get; set; }

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

        public string? SpecialityName { get; set; }
        public string? SpecialityNameAr { get; set; }

        public string? DoctorImg { get; set; }

        public bool? IsActive { get; set; }

    }
}
