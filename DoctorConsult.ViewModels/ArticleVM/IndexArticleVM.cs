using DoctorConsult.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.ViewModels.ArticleVM
{
    public class IndexArticleVM
    {

        public List<GetData>? Results { get; set; }
        public int? Count { get; set; }

        public class GetData
        {
            public int Id { get; set; }
            public string? Title { get; set; }

            public string? TitleAr { get; set; }
            public string? ArticleImg { get; set; }
            public DateTime? Date { get; set; }
            public int? SpecialistId { get; set; }
            public int? OrderId { get; set; }
            public string? SpecialityName { get; set; }
            public string? SpecialityNameAr { get; set; }

            public bool? IsActive { get; set; }
        }
    }
}
