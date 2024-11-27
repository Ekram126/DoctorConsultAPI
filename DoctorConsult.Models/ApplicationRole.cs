using Microsoft.AspNetCore.Identity;

namespace DoctorConsult.Models
{
    public class ApplicationRole : IdentityRole
    {

    }


    public static class Role
    {
        public const string Admin = "Admin";
        public const string SupervisorDoctor = "SupervisorDoctor";
        public const string Doctor = "Doctor";
        public const string Patient = "Patient";
    }

}