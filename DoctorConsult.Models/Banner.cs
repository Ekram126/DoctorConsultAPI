using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.Models
{
    public class Banner
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

        public int? OrderId { get; set; }

        public DateTime? BannerDate { get; set; }
        public int? SpecialistId { get; set; }
        [ForeignKey("SpecialistId")]
        public virtual Specialist? Specialist { get; set; }



    }
}
