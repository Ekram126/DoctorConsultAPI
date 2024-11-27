
using DoctorConsult.ViewModels.BannerVM;
using DoctorConsult.ViewModels.UserVM;

namespace BannerConsult.Domain.Interfaces
{
    public interface IBannerRepository
    {
        List<IndexBannerVM.GetData> GetAll();
     //   Task<List<IndexUserVM.GetData>> GetBannersBySpecialityId(int specialityId);
        Task<IndexBannerVM> GetAll(SortAndFilterBannerVM data, int pageNumber, int pageSize);
        EditBannerVM GetById(int id);
        int Add(CreateBannerVM BannerObj);
        int Update(EditBannerVM BannerObj);
        int Delete(int id);

       // Task<List<IndexUserVM.GetData>> CheckBannerRole(BannerUserRole BannerUserRoleObj);

        public int UpdateBannerImageAfterInsert(CreateBannerVM modelObj);

    }
}
