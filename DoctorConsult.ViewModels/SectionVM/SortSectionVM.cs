using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.ViewModels.SectionVM
{
    public class SortSectionVM
    {
        public int Id { get; set; }

        public string? Name { get; set; }


        public string? NameAr { get; set; }

        public string? Brief { get; set; }
        public string? BriefAr { get; set; }

        public string? SectionImg { get; set; }
        public string? SortStatus { get; set; }
        public string? SortBy { get; set; }
    }
}
