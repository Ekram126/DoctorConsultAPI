using System;
using System.ComponentModel.DataAnnotations;

namespace  DoctorConsult.ViewModels.BannerVM
{
    public class EditBannerVM
    {

        public int Id { get; set; }

        public string? Code { get; set; }
    
        public string? Name { get; set; }

        public string? NameAr { get; set; }

        public string? Brief { get; set; }
        public string? BriefAr { get; set; }

        public string? BannerImg { get; set; }
        public bool? IsActive { get; set; }
        public int? SpecialistId { get; set; }

        public int? OrderId { get; set; }

        public DateTime? BannerDate { get; set; }

    }
}
