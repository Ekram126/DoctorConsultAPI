using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace DoctorConsult.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {

        private IContactUsRepository _contactUsRepository;

        public ContactUsController(IContactUsRepository contactUsRepository)
        {
            _contactUsRepository = contactUsRepository;
        }

        [HttpPost]
        [Route("CreateContactUs")]
        public int CreateContactUs(ContactU contactObj)
        {
            return _contactUsRepository.Add(contactObj);
        }

    }
}
