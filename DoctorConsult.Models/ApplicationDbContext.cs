using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DoctorConsult.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Specialist> Specialists { get; set; }
        public DbSet<Article> Articles { get; set; }

        public DbSet<Video> Videos { get; set; }

        public DbSet<Banner> Banners { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestTracking> RequestTrackings { get; set; }
        public DbSet<RequestStatus> RequestStatus { get; set; }
        public DbSet<RequestDocument> RequestDocuments { get; set; }

        public DbSet<PersonalData> PersonalDatas { get; set; }
        public DbSet<ContactU> ContactUs { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Country> Countries { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


    }
}
