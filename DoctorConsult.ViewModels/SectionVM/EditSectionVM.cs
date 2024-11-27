using DoctorConsult.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorConsult.ViewModels.SectionVM
{
    public class EditSectionVM
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        
        public string? TitleAr { get; set; }

        public string? Brief { get; set; }
        public string? BriefAr { get; set; }

        public string? SectionDesc { get; set; }
        public string? SectionDescAr { get; set; }
        public string? SectionImg { get; set; }


        public bool? IsInAbout { get; set; }

    }
}
