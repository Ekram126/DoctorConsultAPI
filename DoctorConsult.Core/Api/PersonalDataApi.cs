using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;



namespace DoctorConsult.Core.Repositories
{
    public class PersonalDataApi : IPersonalDataRepository
    {
        private ApplicationDbContext _context;
   
        public PersonalDataApi(ApplicationDbContext context)
        {
            _context = context;
        }

        public PersonalData GetPersonalData()
        {
          return  _context.PersonalDatas.FirstOrDefault();
        }
    }
}
