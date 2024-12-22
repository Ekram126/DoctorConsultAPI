using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  DoctorConsult.ViewModels.RequestVM
{
    public class CreateRequestVM
    {
        public int Id { get; set; }
        public string? Subject { get; set; }
        public string? RequestCode { get; set; }
        public DateTime RequestDate { get; set; }
        public string? StrRequestDate { get; set; }

        public string? Complain { get; set; }
        public string? CreatedById { get; set; }
        public int? SpecialityId { get; set; }
        public bool IsRead { get; set; }




    }
}
