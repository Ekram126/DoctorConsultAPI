using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.ViewModels.BannerVM
{
    public class SortBannerVM
    {

        public int Id { get; set; }

        [StringLength(5)]
        public string? Code { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }

        [StringLength(50)]
        public string? NameAr { get; set; }

        public string? Brief { get; set; }
        public string? BriefAr { get; set; }

        public string? BannerImg { get; set; }
        public bool? IsActive { get; set; }
        public int? SpecialistId { get; set; }

        public string? SortStatus { get; set; }

        public string? SortBy { get; set; }
    }
}
