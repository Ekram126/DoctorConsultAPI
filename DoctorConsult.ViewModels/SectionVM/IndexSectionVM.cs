using DoctorConsult.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.ViewModels.SectionVM
{
    public class IndexSectionVM
    {

        public List<GetData>? Results { get; set; }
        public int? Count { get; set; }

        public class GetData
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
}
