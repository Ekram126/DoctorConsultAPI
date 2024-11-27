

namespace DoctorConsult.ViewModels.SectionVM
{
    public class CreateSectionVM
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? TitleAr { get; set; }
        public string? Brief { get; set; }
        public string? BriefAr { get; set; }

        public string? SectionDesc { get; set; }
        public string? SectionDescAr { get; set; }
        public string? SectionImg { get; set; }

        public bool? IsInAbout { get; set; }

    }
}
