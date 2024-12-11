using DoctorConsult.ViewModels.RequestDocumentVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorConsult.ViewModels.RequestVM
{
    public class ViewRequestVM
    {


        public int Id { get; set; }
        public string? Subject { get; set; }
        public string? RequestCode { get; set; }
        public DateTime RequestDate { get; set; }
        public string? UserName { get; set; }
        public string UserId { get; set; }
        public string? Complain { get; set; }

        public string? SpecialityName { get; set; }
        public string? SpecialityNameAr { get; set; }

        public int SpecialityId { get; set; }

        public int? StatusId { get; set; }
        public List<IndexRequestDocumentVM>? listDocuments { get; set; }



    }
}
