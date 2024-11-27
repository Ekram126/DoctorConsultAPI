using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.Models
{
    public class RequestTracking
    {
        public int Id { get; set; }
        public string? Advice { get; set; }
        public DateTime? ResponseDate { get; set; }
        public int? StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual RequestStatus? RequestStatus { get; set; }
        public int RequestId { get; set; }
        [ForeignKey("RequestId")]
        public virtual Request? Request { get; set; }
        public string? CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public virtual ApplicationUser? User { get; set; }

        public string? AssignTo { get; set; }
        [ForeignKey("AssignTo")]
        public virtual ApplicationUser? AssignToUser { get; set; }

    }
}
