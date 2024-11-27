using  DoctorConsult.ViewModels.RequestTrackingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  DoctorConsult.ViewModels.RequestVM
{
    public class SortRequestVM
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public string? RequestCode { get; set; }
        public string? Subject { get; set; }
        public string? RequestDate { get; set; }
        public string? StatusName { get; set; }
        public string? StatusNameAr { get; set; }
        public string? StatusColor { get; set; }
        public string? StatusIcon { get; set; }
        public string? ClosedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? SortStatus { get; set; }
        public string? Description { get; set; }
   public string? SortBy { get; set; }

  


    }
}
