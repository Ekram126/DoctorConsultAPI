using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using Microsoft.EntityFrameworkCore;

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

        public int SavePersonalData(PersonalData model)
        {
            try
            {
                if (model != null)
                {
                    PersonalData personalObj = new PersonalData();

                    personalObj.Email = model.Email;
                    personalObj.Address = model.Address;
                    personalObj.AddressAr = model.AddressAr;
                    personalObj.Mobile = model.Mobile;
                    personalObj.WhatsApp = model.WhatsApp;
                    _context.PersonalDatas.Add(personalObj);
                    _context.SaveChanges();
                    return personalObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;
        }

        public int UpdatePersonalData(PersonalData model)
        {
            try
            {
                var personalObj = _context.PersonalDatas.Find(model.Id);
                personalObj.Email = model.Email;
                personalObj.Address = model.Address;
                personalObj.AddressAr = model.AddressAr;
                personalObj.Mobile = model.Mobile;
                personalObj.WhatsApp = model.WhatsApp;

                _context.Entry(personalObj).State = EntityState.Modified;
                _context.SaveChanges();
                return personalObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }
    }
}
