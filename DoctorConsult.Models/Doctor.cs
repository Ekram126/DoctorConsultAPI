using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.Models
{
   public class Doctor
    {
        public int Id { get; set; }

        [StringLength(5)]
        public string? Code { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }

        [StringLength(50)]
        public string? NameAr { get; set; }

        [StringLength(14)]
        public string? NationalId { get; set; }

        public string? Mobile { get; set; }

        public DateTime? Dob { get; set; }

        [StringLength(320)]
        public string? Email { get; set; }
        public string? Address { get; set; }

        public string? AddressAr { get; set; }
        public DateTime? GradDate { get; set; }

        public DateTime? JoinDate { get; set; }

        public string? Remarks { get; set; }
        public int? GenderId { get; set; }

      public string? DoctorImg { get; set; }
     

        public bool? IsActive { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Doctor? SupervisorDoctor { get; set; }

        public int? SpecialistId { get; set; }
        [ForeignKey("SpecialistId")]
        public virtual Specialist? Specialist { get; set; }



    }
}
