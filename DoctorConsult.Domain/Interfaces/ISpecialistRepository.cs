using DoctorConsult.Models;
using DoctorConsult.ViewModels.RequestVM;
using DoctorConsult.ViewModels.SpecialistVM;

namespace DoctorConsult.Domain.Interfaces
{
    public interface ISpecialistRepository
    {
        List<Specialist> GetAll();
        IndexSpecialistVM GetAll(SortAndFilterSpecialistVM data, int pageNumber, int pageSize);
        EditSpecialistVM GetById(int id);
        int Add(CreateSpecialistVM DoctorObj);
        int Update(EditSpecialistVM DoctorObj);
        int Delete(int id);

        GenerateSpecialityNumberVM GenerateSpecialityNumber();

        IEnumerable<Specialist> AutoCompleteSpecialityName(string name);

    }
}
