using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.ViewModels.ArticleVM
{
    public class SortArticleVM
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? TitleAr { get; set; }
        public string? ArticleImg { get; set; }
        public DateTime? Date { get; set; }
        public int? SpecialistId { get; set; }
        public string? SortStatus { get; set; }
        public string? SortBy { get; set; }
    }
}
