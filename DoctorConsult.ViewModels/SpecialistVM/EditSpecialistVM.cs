using System;

namespace  DoctorConsult.ViewModels.SpecialistVM
{
    public class EditSpecialistVM
    {
        public int Id { get; set; }

        public int? Code { get; set; }

        public string? Name { get; set; }


        public string? NameAr { get; set; }

        public string? SVGIcon { get; set; }
        public string? PNGIcon { get; set; }

        public bool? IsActive { get; set; }
    }
}
