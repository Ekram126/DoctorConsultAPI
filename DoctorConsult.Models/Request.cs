using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string? Subject { get; set; }
        public string? RequestCode { get; set; }
        public string? Complain { get; set; }
        public DateTime RequestDate { get; set; }

      


        public string? CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public virtual ApplicationUser? User { get; set; }


        public int? SpecialityId { get; set; }
        [ForeignKey("SpecialityId")]
        public virtual Specialist? Specialist { get; set; }

    }
}
