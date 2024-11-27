using DoctorConsult.ViewModels.ArticleVM;
using DoctorConsult.ViewModels.VideoVM;

namespace DoctorConsult.Domain.Interfaces
{
    public interface IVideoRepository
    {
        List<IndexVideoVM.GetData> GetAll();

        List<IndexVideoVM.GetData> GetActivatedVideos();
        List<IndexVideoVM.GetData> GetActivatedVideosBySpecialityId(int specialityId);
        Task<IndexVideoVM> GetAll(SortAndFilterVideoVM data, int pageNumber, int pageSize);
        EditVideoVM GetById(int id);
        int Add(CreateVideoVM ArticleObj);
        int Update(EditVideoVM ArticleObj);
        int Delete(int id);
    }
}
