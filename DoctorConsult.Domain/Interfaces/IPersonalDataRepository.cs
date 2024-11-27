using DoctorConsult.Models;
using DoctorConsult.ViewModels.ArticleVM;

namespace DoctorConsult.Domain.Interfaces
{
    public interface IPersonalDataRepository
    {
        PersonalData GetPersonalData();
    }
}
