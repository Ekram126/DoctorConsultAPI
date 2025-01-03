﻿

namespace  DoctorConsult.ViewModels.RequestVM
{
    public class EditRequestVM
    {
        public int Id { get; set; }
        public string? Subject { get; set; }
        public string? RequestCode { get; set; }
        public string? Description { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? RequestTime { get; set; }
        public int RequestModeId { get; set; }
        public int? SubProblemId { get; set; }
        public int?  DoctorConsultDetailId { get; set; }
        public string? SerialNumber { get; set; }
        public int RequestPeriorityId { get; set; }
        public string? CreatedById { get; set; }
        public int? RequestTypeId { get; set; }
        public int? HospitalId { get; set; }
        public bool IsRead { get; set; }

    }
}
