using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  DoctorConsult.ViewModels.BannerVM
{
    public class SearchBannerVM
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
        public string? UserId { get; set; }

    }
}
