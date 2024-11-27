using DoctorConsult.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorConsult.ViewModels.VideoVM
{
    public class EditVideoVM
    {
        public int Id { get; set; }

        public string? VideoURL { get; set; }
        public string? Title { get; set; }
        public string? TitleAr { get; set; }


        public string? Brief { get; set; }
        public string? BriefAr { get; set; }


        public DateTime? Date { get; set; }

        public int? OrderId { get; set; }

        public int? SpecialityId { get; set; }

        public bool? IsActive { get; set; }


        public string? SpecialityName { get; set; }
        public string? SpecialityNameAr { get; set; }



    }
}
