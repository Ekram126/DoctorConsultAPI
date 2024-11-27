using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.Models
{
    public class DoctorDocument
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string? Title { get; set; }

        [StringLength(25)]
        public string? FileName { get; set; }
        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Doctor? Doctor { get; set; }

    }
}
