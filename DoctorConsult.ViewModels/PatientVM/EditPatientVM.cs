using System;

namespace DoctorConsult.ViewModels.PatientVM
{
    public class EditPatientVM
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

        public int? GenderId { get; set; }

        public int? CountryId { get; set; }
    }
}
