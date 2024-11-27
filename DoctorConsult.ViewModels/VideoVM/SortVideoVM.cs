using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.ViewModels.VideoVM
{
    public class SortVideoVM
    {
        public int Id { get; set; }

        public string? VideoURL { get; set; }

        public string? Title { get; set; }
        public string? TitleAr { get; set; }


        public string? Brief { get; set; }
        public string? BriefAr { get; set; }

        public DateTime? Date { get; set; }

        public int? OrderId { get; set; }

        public int? SpecialityId { get; set; }

        public bool? IsActive { get; set; }
        public string? SortStatus { get; set; }
        public string? SortBy { get; set; }
    }
}
