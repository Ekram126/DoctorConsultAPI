
using DoctorConsult.Models;
using DoctorConsult.ViewModels.CountryVM;

namespace DoctorConsult.Domain.Interfaces
{
    public interface ICountryRepository
    {
        List<IndexCountryVM.GetData> GetAll();

       string GetPhoneCodeByCountryId(int countryId);
    }
}
