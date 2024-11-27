using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  DoctorConsult.ViewModels.SpecialistVM
{
    public class IndexSpecialistVM
    {

        public List<GetData>? Results { get; set; }
        public int? Count { get; set; }

        public class GetData
        {
            public int Id { get; set; }

            public string? Code { get; set; }
            public string? Name { get; set; }
            public string? NameAr { get; set; }


            public string? SVGIcon { get; set; }
            public string? PNGIcon { get; set; }


            public bool? IsActive { get; set; }


        }
    }
}
