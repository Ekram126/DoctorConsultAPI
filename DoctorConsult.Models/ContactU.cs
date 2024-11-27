using System;
using System.Collections.Generic;

#nullable disable

namespace DoctorConsult.Models
{
    public partial class ContactU
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
    }
}
