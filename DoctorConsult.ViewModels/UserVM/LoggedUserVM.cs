using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.ViewModels.UserVM
{
    public class LoggedUserVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
      
       public string Token { get; set; }
        public List<string> RoleNames { get; set; }
    }
}
