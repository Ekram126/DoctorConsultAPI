using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  DoctorConsult.ViewModels.RequestTrackingVM
{
    public class IndexRequestTrackingVM
    {

        public List<GetData>? Results { get; set; }
        public int? Count { get; set; }

        public class GetData
        {
            public int Id { get; set; }
            public string? Subject { get; set; }
            public string? Advice { get; set; }
            public string? StrResponseDate { get; set; }

            public DateTime? ResponseDate { get; set; }
            public int? StatusId { get; set; }
            public string? StatusName { get; set; }
            public string? StatusNameAr { get; set; }
            public string? StatusColor { get; set; }
            public string? StatusIcon { get; set; }

            public int? RequestId { get; set; }
            public string? CreatedByName { get; set; }
            public string? AssignedToUser { get; set; }
        }
    }









    
}
