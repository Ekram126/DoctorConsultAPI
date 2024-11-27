using DoctorConsult.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  DoctorConsult.ViewModels.RequestStatusVM
{
    public class IndexRequestStatusVM
    {
        public List<GetData>? Results { get; set; }

        public int? CountOpen { get; set; }
        public int? CountClosed { get; set; }
        public int? CountInProgress { get; set; }
        public int? CountSolved { get; set; }
        public int? CountAll { get; set; }
        public int? CountApproved { get; set; }
        public List<RequestStatus>? ListStatus { get; set; }

        public class GetData
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? NameAr { get; set; }
            public string? Color { get; set; }
            public string? Icon { get; set; }
            public string? CreatedBy { get; set; }
            public int? RequestId { get; set; }
            public int? CountOpen { get; set; }
            public int? CountClosed { get; set; }
            public int? CountInProgress { get; set; }
            public int? CountSolved { get; set; }
            public int? CountAll { get; set; }
            public int? CountApproved { get; set; }

    }
    }
}
