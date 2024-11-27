using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;



namespace DoctorConsult.Core.Repositories
{
    public class ContactUsApi : IContactUsRepository
    {
        private ApplicationDbContext _context;
   
        public ContactUsApi(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add Contact Us
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public int Add(ContactU model)
        {

            try
            {
                if (model != null)
                {
                    ContactU contactUsObj = new ContactU();
                    contactUsObj.FullName = model.FullName;
                    contactUsObj.Email = model.Email;
                    contactUsObj.Phone = model.Phone;
                    contactUsObj.Message = model.Message;
                    _context.ContactUs.Add(contactUsObj);
                    _context.SaveChanges();
                    return contactUsObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;

        }


    }
}
