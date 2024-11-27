using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.Models
{
    public class Section
    {
        public int Id { get; set; }


        [StringLength(50)]
        public string? Title { get; set; }

        [StringLength(50)]
        public string? TitleAr { get; set; }

        public string? Brief { get; set; }
        public string? BriefAr { get; set; }



        public string? SectionDesc { get; set; }
        public string? SectionDescAr { get; set; }

        public bool? IsInAbout { get; set; }


        public string? SectionImg { get; set; }




    }
}
