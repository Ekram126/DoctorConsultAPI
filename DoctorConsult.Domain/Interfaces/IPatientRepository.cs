

using DoctorConsult.Models;
using DoctorConsult.ViewModels.DoctorVM;
using DoctorConsult.ViewModels.PatientVM;
using DoctorConsult.ViewModels.RequestVM;

namespace DoctorConsult.Domain.Interfaces
{
    public interface IPatientRepository
    {

        List<Patient> GetAll();

      //  Task<IndexPatientVM> ListPatients(SortAndFilterPatientVM data);
        Task<IndexPatientVM> GetAll(SortAndFilterPatientVM data, int pageNumber, int pageSize);
        EditPatientVM GetById(int id);
        int Add(CreatePatientVM PatientObj);
        int Update(EditPatientVM PatientObj);
        int Delete(int id);
        GeneratedPatientNumberVM GeneratePatientNumber();


    }
}
