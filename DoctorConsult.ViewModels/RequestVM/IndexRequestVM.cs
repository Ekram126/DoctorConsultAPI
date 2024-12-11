using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace  DoctorConsult.ViewModels.RequestVM
{
    public class IndexRequestVM
    {
        public List<GetData>? Results { get; set; }

        public int Count { get; set; }  
        

        public class GetData
        {
            public int Id { get; set; }
            public string? Subject { get; set; }
            public string? RequestCode { get; set; }
            public DateTime RequestDate { get; set; }
            public DateTime? ActionDate { get; set; }
            public string? CreatedBy { get; set; }
            public string? UserName { get; set; }

            public string? Advice { get; set; }
            public string? Complain { get; set; }

            public int StatusId { get; set; }
            public string? StatusName { get; set; }
            public string? StatusNameAr { get; set; }
            public string? StatusColor { get; set; }
            public string? StatusIcon { get; set; }


            public string? SpecialityName { get; set; }
            public string? SpecialityNameAr { get; set; }

        }
    }
}
