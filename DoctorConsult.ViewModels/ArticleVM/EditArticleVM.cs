using DoctorConsult.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorConsult.ViewModels.ArticleVM
{
    public class EditArticleVM
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
        public int? SpecialityId { get; set; }
        public int? OrderId { get; set; }



        public string? SpecialityName { get; set; }
        public string? SpecialityNameAr { get; set; }

        public bool? IsActive { get; set; }



    }
}
