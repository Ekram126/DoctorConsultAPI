using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.ArticleVM;
using DoctorConsult.ViewModels.CountryVM;
using DoctorConsult.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorConsult.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class CountryController : ControllerBase
    {

        private ICountryRepository _countryRepository;

        public CountryController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }








        [HttpGet]
        [Route("ListAllCountries")]
        public List<IndexCountryVM.GetData> GetAll()
        {
            return _countryRepository.GetAll();
        }



        [HttpGet]
        [Route("GetPhoneCodeByCountryId/{countryId}")]
        public string GetPhoneCodeByCountryId(int countryId)
        {
            return _countryRepository.GetPhoneCodeByCountryId(countryId);
        }

    }
}
