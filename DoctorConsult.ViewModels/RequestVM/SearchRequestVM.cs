
namespace  DoctorConsult.ViewModels.RequestVM
{
    public class SearchRequestVM
    {

        public int? StatusId { get; set; }

        public int? SpecialityId { get; set; }
        public string? Subject { get; set; }
        public string? Code { get; set; }
        public string? UserId { get; set; }
        public string? Start { get; set; }
        public string? End { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        public string? Lang { get; set; }


        public string? StrStartDate { get; set; }
        public string? StrEndDate { get; set; }
        public string? StatusName { get; set; }
        public string? StatusNameAr { get; set; }
        public string? PrintedBy { get; set; }


    }
}
