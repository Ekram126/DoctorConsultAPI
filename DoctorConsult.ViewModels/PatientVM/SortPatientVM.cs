﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  DoctorConsult.ViewModels.PatientVM
{
    public class SortPatientVM
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? NameAr { get; set; }
        public string? Email { get; set; }

        public string? SortStatus { get; set; }

        public string? SortBy { get; set; }

    }
}