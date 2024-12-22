using DoctorConsult.ViewModels.SpecialistVM;
using DoctorConsult.Models;
using DoctorConsult.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using DoctorConsult.ViewModels.DoctorVM;
using Microsoft.AspNetCore.Identity;
using DoctorConsult.ViewModels.RequestVM;

namespace SpecialistConsult.Core.Repositories
{
    public class SpecialistApi : ISpecialistRepository
    {
        private ApplicationDbContext _context;


        public SpecialistApi(ApplicationDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Add the speciality model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public int Add(CreateSpecialistVM model)
        {

            try
            {
                if (model != null)
                {
                    Specialist specialistObj = new Specialist();
                    specialistObj.Code = model.Code;
                    specialistObj.Name = model.Name;
                    specialistObj.NameAr = model.NameAr;
                    specialistObj.SVGIcon = model.SVGIcon;
                    specialistObj.PNGIcon = model.PNGIcon;
                    specialistObj.IsActive = model.IsActive;
                    _context.Specialists.Add(specialistObj);
                    _context.SaveChanges();
                    return specialistObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;

        }

        /// <summary>
        /// Delete the speciality by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public int Delete(int id)
        {
            var specialistObj = _context.Specialists.Find(id);
            try
            {
                _context.Specialists.Remove(specialistObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        /// <summary>
        /// Gets all specialities.
        /// </summary>
        /// <returns></returns>
        public List<Specialist> GetAll()
        {
            return _context.Specialists.Where(a=>a.IsActive == true).OrderBy(a=>a.Code).ToList();
        }

        /// <summary>
        /// Gets all specialities.
        /// </summary>
        /// <param name="data">The data sort and filter.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public IndexSpecialistVM GetAll(SortAndFilterSpecialistVM data, int pageNumber, int pageSize)
        {
            IndexSpecialistVM mainClass = new IndexSpecialistVM();
            List<IndexSpecialistVM.GetData> list = new List<IndexSpecialistVM.GetData>();
            var lstSpecialists = _context.Specialists.ToList();

            List<string> lstRoleNames = new List<string>();
            var userObj = new ApplicationUser();


            #region Search Criteria
            if (data.SearchObj?.Id != 0)
            {
                lstSpecialists = lstSpecialists.Where(x => x.Id == data.SearchObj?.Id).ToList();
            }

            #endregion


            #region Sort Criteria

            switch (data.SortObj?.SortBy)
            {
                case "Name":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstSpecialists = lstSpecialists.OrderBy(x => x.Name).ToList();
                    }
                    else
                    {
                        lstSpecialists = lstSpecialists.OrderByDescending(x => x.Name).ToList();
                    }
                    break;
                case "الاسم":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstSpecialists = lstSpecialists.OrderBy(x => x.NameAr).ToList();
                    }
                    else
                    {
                        lstSpecialists = lstSpecialists.OrderByDescending(x => x.NameAr).ToList();
                    }
                    break;

                case "Code":
                case "الكود":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstSpecialists = lstSpecialists.OrderBy(x => x.Code).ToList();
                    }
                    else
                    {
                        lstSpecialists = lstSpecialists.OrderByDescending(x => x.Code).ToList();
                    }
                    break;


            }

            #endregion

            var countItems = lstSpecialists.ToList();
            mainClass.Count = countItems.Count();
            if (pageNumber == 0 && pageSize == 0)
                lstSpecialists = lstSpecialists.ToList();
            else
                lstSpecialists = lstSpecialists.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            if (lstSpecialists.Count > 0)
            {
                foreach (var item in lstSpecialists)
                {
                    IndexSpecialistVM.GetData getDataObj = new IndexSpecialistVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Code = item.Code;
                    getDataObj.Name = item.Name;
                    getDataObj.NameAr = item.NameAr;
                    getDataObj.PNGIcon = item.PNGIcon;
                    getDataObj.IsActive = item.IsActive;
                    list.Add(getDataObj);
                }
            }
            mainClass.Results = list;
            return mainClass;
        }

        /// <summary>
        /// Get speciality the by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public EditSpecialistVM GetById(int id)
        {
            return _context.Specialists.Where(a => a.Id == id).Select(item => new EditSpecialistVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr,
                SVGIcon = item.SVGIcon,
                PNGIcon = item.PNGIcon,
                IsActive = item.IsActive
        }).First();
        }

        /// <summary>
        /// Updates the Speciality model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public int Update(EditSpecialistVM model)
        {
            try
            {
                var specialistObj = _context.Specialists.Find(model.Id);
                specialistObj.Id = model.Id;
                specialistObj.Code = model.Code;
                specialistObj.Name = model.Name;
                specialistObj.NameAr = model.NameAr;
                specialistObj.SVGIcon = model.SVGIcon;
                specialistObj.PNGIcon = model.PNGIcon;
                specialistObj.IsActive = model.IsActive;
                _context.Entry(specialistObj).State = EntityState.Modified;
                _context.SaveChanges();
                return specialistObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        /// <summary>
        /// Generates the speciality number.
        /// </summary>
        /// <returns></returns>
        public GenerateSpecialityNumberVM GenerateSpecialityNumber()
        {
            GenerateSpecialityNumberVM numberObj = new GenerateSpecialityNumberVM();
            string WO = "";
            int desiredLength = 3; // Adjust the desired length as needed

            var lstIds = _context.Specialists.ToList();
            if (lstIds.Count > 0)
            {
                int? code = lstIds?.LastOrDefault()?.Code;
                // Convert the code to a number and increment
                int incrementedCode = int.Parse(code.ToString()) + 1;
                numberObj.SprcialityCode = WO + incrementedCode.ToString().PadLeft(desiredLength, '0');
            }
            else
            {
                numberObj.SprcialityCode = WO + 1.ToString().PadLeft(desiredLength, '0');
            }

            return numberObj;
        }

        /// <summary>
        /// Automatics the name of the complete speciality.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IEnumerable<Specialist> AutoCompleteSpecialityName(string name)
        {
            var lst = _context.Specialists.Where(a => a.Name.Contains(name) || a.NameAr.Contains(name)).ToList();
            return lst;
        }
    }
}
