
using DoctorConsult.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.ViewModels.ArticleVM
{
    public class SearchArticleVM
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? TitleAr { get; set; }


        public string? ArticleImg { get; set; }
        public DateTime? Date { get; set; }
        public int? SpecialityId { get; set; }


        public bool? IsActive { get; set; }



        public string? Start { get; set; }
        public string? End { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
