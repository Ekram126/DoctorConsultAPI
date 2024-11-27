using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace   DoctorConsult.Models
{
    public class Specialist
    {

        public int Id { get; set; }
        public string? Code { get; set; }


        [StringLength(50)]
        public string? Name { get; set; }
        [StringLength(50)]
        public string? NameAr { get; set; }

        public string? SVGIcon { get; set; }
        public string? PNGIcon { get; set; }

        public bool? IsActive { get; set; }
    }
}
