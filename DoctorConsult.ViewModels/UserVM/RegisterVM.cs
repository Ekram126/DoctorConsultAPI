﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.ViewModels.UserVM
{
  public  class RegisterVM
    {
        public string? Id { get; set; }

        public string? Username { get; set; }

        public string? PasswordHash { get; set; }



        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
