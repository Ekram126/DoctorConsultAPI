using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.ArticleVM;
using DoctorConsult.ViewModels.CountryVM;
using Microsoft.EntityFrameworkCore;


namespace ArticleConsult.Core.Repositories
{
    public class CountryApi : ICountryRepository
    {
        private ApplicationDbContext _context;

        public CountryApi(ApplicationDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Gets all countries.
        /// </summary>
        /// <returns></returns>
        public List<IndexCountryVM.GetData> GetAll()
        {
            List<IndexCountryVM.GetData> list = new List<IndexCountryVM.GetData>();

            var countries = _context.Countries.ToList();

            if (countries.Count > 0)
            {
                foreach (var item in countries)
                {
                    IndexCountryVM.GetData getDataObj = new IndexCountryVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Name = item.Name;
                    getDataObj.NameAr = item.NameAr;
                    list.Add(getDataObj);
                }
            }

            return list;
        }



        /// <summary>
        /// Gets the phone code by country identifier.
        /// </summary>
        /// <param name="countryId">The country identifier.</param>
        /// <returns></returns>
        public string GetPhoneCodeByCountryId(int countryId)
        {
            var lstCountries = _context.Countries.Where(a => a.Id == countryId).ToList();
            if (lstCountries.Count > 0)
            {
                return lstCountries[0].PhoneCode;
            }
            else
            {
                return "";
            }
        }
    }
}
