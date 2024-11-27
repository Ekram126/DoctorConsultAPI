
using DoctorConsult.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.ViewModels.SectionVM
{
    public class SearchSectionVM
    {
        public int Id { get; set; }


   
        public string? Name { get; set; }


        public string? NameAr { get; set; }

        public string? Brief { get; set; }
        public string? BriefAr { get; set; }

        public string? SectionImg { get; set; }


    }
}
