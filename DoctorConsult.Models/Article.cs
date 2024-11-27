using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.Models
{
    public class Article
    {
        public int Id { get; set; }


        public string? Title { get; set; }
        public string? TitleAr { get; set; }
        public string? ArticleContent { get; set; }
        public string? ArticleContentAr { get; set; }

        public string? ArticleDesc { get; set; }
        public string? ArticleDescAr { get; set; }
        public string? ArticleImg { get; set; }

        public DateTime? Date { get; set; }

        public int? OrderId { get; set; }

        public int? SpecialityId { get; set; }
        [ForeignKey("SpecialityId")]
        public Specialist? Specialist { get; set; }


        public bool? IsActive { get; set; }

    }
}
