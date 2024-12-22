using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  DoctorConsult.ViewModels.RequestTrackingVM
{
    public class EditRequestTrackingVM
    {
        public int Id { get; set; }
        public string? Advice { get; set; }
        public string? StrResponseDate { get; set; }

        public DateTime? ResponseDate { get; set; }
        public int? StatusId { get; set; }
        public int? RequestId { get; set; }
        public string? DoctorId { get; set; }
        public string? ManagerDoctorId { get; set; }

    }
}
