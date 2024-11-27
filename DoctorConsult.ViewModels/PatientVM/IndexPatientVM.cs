using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.ViewModels.PatientVM
{
    public class IndexPatientVM
    {

        public List<GetData>? Results { get; set; }

        public int Count { get; set; }
        public class GetData
        {
            public int Id { get; set; }
            public string? Code { get; set; }
            public string? Name { get; set; }
            public string? NameAr { get; set; }
            public string? Email { get; set; }
            public string? NationalId { get; set; }
            public string? Mobile { get; set; }

            public string? CountryName { get; set; }
            public string? CountryNameAr { get; set; }
        }
    }
}
