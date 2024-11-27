using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorConsult.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalDataController : ControllerBase
    {


        IPersonalDataRepository _personalDataRepository;

        public PersonalDataController(IPersonalDataRepository personalDataRepository)
        {
            _personalDataRepository = personalDataRepository;

        }






        [HttpGet]
        [Route("GetPersonalData")]
        public PersonalData GetPersonalData()
        {
            return _personalDataRepository.GetPersonalData();
        }
    }
}
