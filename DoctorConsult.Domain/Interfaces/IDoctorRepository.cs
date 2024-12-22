using DoctorConsult.Models;
using DoctorConsult.ViewModels.ArticleVM;
using DoctorConsult.ViewModels.DoctorVM;
using DoctorConsult.ViewModels.RequestVM;
using DoctorConsult.ViewModels.UserVM;

namespace DoctorConsult.Domain.Interfaces
{
    public interface IDoctorRepository
    {
        List<IndexDoctorVM.GetData> GetAll();
        Task<List<IndexUserVM.GetData>> GetDoctorsBySpecialityId(int specialityId);
        Task<IndexDoctorVM> GetAll(SortAndFilterDoctorVM data, int pageNumber, int pageSize);
        EditDoctorVM GetById(int id);
        int Add(CreateDoctorVM DoctorObj);
        int Update(EditDoctorVM DoctorObj);
        int Delete(int id);

        Task<List<IndexUserVM.GetData>> CheckDoctorRole(DoctorUserRole doctorUserRoleObj);

        public int UpdateDoctorImageAfterInsert(CreateDoctorVM modelObj);
        GeneratedDoctorCodeVM GenerateDoctorCode();

    }
}
