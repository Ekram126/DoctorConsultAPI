using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  DoctorConsult.ViewModels.RequestTrackingVM
{
    public class CreateRequestTrackingVM
    {
        public int Id { get; set; }
        public string? Advice { get; set; }
        public string? StrRespondDate { get; set; }

        public DateTime ResponseDate { get; set; }
        public int? StatusId { get; set; }
        public int? RequestId { get; set; }
        public string? CreatedById { get; set; }
        public string? AssignTo { get; set; }
    }
}
