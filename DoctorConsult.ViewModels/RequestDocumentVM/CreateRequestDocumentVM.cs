using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  DoctorConsult.ViewModels.RequestDocumentVM
{
    public class CreateRequestDocumentVM
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? FileName { get; set; }
        public int RequestTrackingId { get; set; }
    }
}
