﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.ViewModels.UserVM
{
    public class IndexUserVM
    {

        public List<GetData> Results { get; set; }


        public class GetData
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }


            public int DoctorId { get; set; }


            public bool? IsActive { get; set; }
        }
    }
}
