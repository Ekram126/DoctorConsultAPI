using DoctorConsult.Models;
using DoctorConsult.ViewModels.ArticleVM;

namespace DoctorConsult.Domain.Interfaces
{
    public interface IArticleRepository
    {
        List<IndexArticleVM.GetData> GetAll();

        List<IndexArticleVM.GetData> GetActivatedArticles();
        List<IndexArticleVM.GetData> GetActivatedArticlesBySpecialityId(int specialityId);
        Task<IndexArticleVM> GetAll(SortAndFilterArticleVM data, int pageNumber, int pageSize);
        EditArticleVM GetById(int id);
        int Add(CreateArticleVM ArticleObj);
        int Update(EditArticleVM ArticleObj);
        int Delete(int id);
        public int UpdateArticleImageAfterInsert(CreateArticleVM modelObj);
    }
}
