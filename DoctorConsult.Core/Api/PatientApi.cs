using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.PatientVM;
using DoctorConsult.ViewModels.RequestVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace DoctorConsult.Core.Repositories
{
    public class PatientApi : IPatientRepository
    {
        private ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;
        RoleManager<ApplicationRole> _roleManager;

        public PatientApi(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Adds the patient model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public int Add(CreatePatientVM model)
        {

            try
            {
                if (model != null)
                {
                    Patient PatientObj = new Patient();
                    PatientObj.Code = model.Code;
                    PatientObj.Name = model.Name;
                    PatientObj.NameAr = model.NameAr;
                    PatientObj.NationalId = model.NationalId;
                    PatientObj.Email = model.Email;
                    PatientObj.Address = model.Address;
                    PatientObj.AddressAr = model.AddressAr;
                    PatientObj.Mobile = model.Mobile;
                    if (model.StrDob != "")
                        PatientObj.Dob = DateTime.Parse(model.StrDob);

                    PatientObj.GenderId = model.GenderId;
                    if (model.CountryId != 0)
                        PatientObj.CountryId = model.CountryId;
                    _context.Patients.Add(PatientObj);
                    _context.SaveChanges();
                    return PatientObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;

        }

        /// <summary>
        /// Deletes the patient specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public int Delete(int id)
        {
            var PatientObj = _context.Patients.Find(id);
            try
            {
                _context.Patients.Remove(PatientObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        /// <summary>
        /// Generates the patient number.
        /// </summary>
        /// <returns></returns>
        public GeneratedPatientNumberVM GeneratePatientNumber()
        {
            GeneratedPatientNumberVM codeObj = new GeneratedPatientNumberVM();
            string str = "Pnt";

            var lstIds = _context.Patients.ToList();
            if (lstIds.Count > 0)
            {
                var code = lstIds.LastOrDefault().Id;
                codeObj.PatientCode = str + DateTime.Today.Date.Year + DateTime.Today.Date.Month + DateTime.Today.Date.Day + (code + 1);
            }
            else
            {
                codeObj.PatientCode = str + DateTime.Today.Date.Year + DateTime.Today.Date.Month + DateTime.Today.Date.Day + 1;
            }

            return codeObj;
        }


        /// <summary>
        /// Gets all patients.
        /// </summary>
        /// <returns></returns>
        public List<Patient> GetAll()
        {
            return _context.Patients.ToList();
        }
        /// <summary>
        /// Gets all patients.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public async Task<IndexPatientVM> GetAll(SortAndFilterPatientVM data, int pageNumber, int pageSize)
        {

            IndexPatientVM mainClass = new IndexPatientVM();
            List<IndexPatientVM.GetData> list = new List<IndexPatientVM.GetData>();
            var lstPatients = _context.Patients.Include(a => a.Country).ToList();


            List<string> lstRoleNames = new List<string>();
            Doctor doctorObj = new Doctor();
            Patient patientObj = new Patient();
            var userObj = new ApplicationUser();

            if (data.SearchObj.UserId != null)
            {
                userObj = await _userManager.FindByIdAsync(data.SearchObj.UserId);

                var roles = (from userRole in _context.UserRoles
                             join role in _roleManager.Roles on userRole.RoleId equals role.Id
                             where userRole.UserId == userObj.Id
                             select role);
                foreach (var role in roles)
                {
                    lstRoleNames.Add(role.Name);
                }


            }


            if (lstRoleNames.Contains("Patient"))
            {
                lstPatients = lstPatients.Where(a => a.Email == userObj.Email).ToList();
            }


            #region Sort Criteria

            switch (data.SortObj?.SortBy)
            {
                case "Name":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstPatients = lstPatients.OrderBy(x => x.Name).ToList();
                    }
                    else
                    {
                        lstPatients = lstPatients.OrderByDescending(x => x.Name).ToList();
                    }
                    break;
                case "الاسم":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstPatients = lstPatients.OrderBy(x => x.NameAr).ToList();
                    }
                    else
                    {
                        lstPatients = lstPatients.OrderByDescending(x => x.NameAr).ToList();
                    }
                    break;

                case "National Identity":
                case "الرقم القومي":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstPatients = lstPatients.OrderBy(x => x.NationalId).ToList();
                    }
                    else
                    {
                        lstPatients = lstPatients.OrderByDescending(x => x.NationalId).ToList();
                    }
                    break;
                case "E-mail":
                case "البريد الإلكتروني":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstPatients = lstPatients.OrderBy(x => x.Email).ToList();
                    }
                    else
                    {
                        lstPatients = lstPatients.OrderByDescending(x => x.Email).ToList();
                    }
                    break;


                case "Mobile":
                case "المحمول":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstPatients = lstPatients.OrderBy(x => x.Mobile).ToList();
                    }
                    else
                    {
                        lstPatients = lstPatients.OrderByDescending(x => x.Mobile).ToList();
                    }
                    break;



            }

            #endregion

            var countItems = lstPatients.ToList();
            mainClass.Count = countItems.Count();
            if (pageNumber == 0 && pageSize == 0)
                lstPatients = lstPatients.ToList();
            else
                lstPatients = lstPatients.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            if (lstPatients.Count > 0)
            {
                foreach (var item in lstPatients)
                {
                    IndexPatientVM.GetData getDataObj = new IndexPatientVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Code = item.Code;
                    getDataObj.Name = item.Name;
                    getDataObj.NameAr = item.NameAr;
                    getDataObj.NationalId = item.NationalId;
                    getDataObj.Mobile = item.Mobile;
                    getDataObj.Email = item.Email;
                    getDataObj.CountryName = item?.Country?.Name;
                    getDataObj.CountryNameAr = item?.Country?.NameAr;
                    list.Add(getDataObj);
                }
            }
            mainClass.Results = list;
            return mainClass;
        }






  

        /// <summary>
        /// Get the patient  by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public EditPatientVM GetById(int id)
        {
            return _context.Patients.Where(a => a.Id == id).Select(item => new EditPatientVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr,
                NationalId = item.NationalId,
                Dob = item.Dob != null ? DateTime.Parse(item.Dob.ToString()) : null,
                Mobile = item.Mobile,
                Email = item.Email,
                Address = item.Address,
                AddressAr = item.AddressAr,
                GenderId = item.GenderId,

                CountryId = item.CountryId,
            }).First();
        }

        /// <summary>
        /// Updates  the patient specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public int Update(EditPatientVM model)
        {
            try
            {
                var PatientObj = _context.Patients.Find(model.Id);
                PatientObj.Id = model.Id;
                PatientObj.Code = model.Code;
                PatientObj.Name = model.Name;
                PatientObj.NameAr = model.NameAr;
                PatientObj.NationalId = model.NationalId;
                if (model.StrDob != "")
                    PatientObj.Dob = DateTime.Parse(model.StrDob.ToString());
                PatientObj.Mobile = model.Mobile;
                PatientObj.Email = model.Email;
                PatientObj.Address = model.Address;
                PatientObj.AddressAr = model.AddressAr;

                PatientObj.GenderId = model.GenderId;
                PatientObj.CountryId = model.CountryId;
                _context.Entry(PatientObj).State = EntityState.Modified;
                _context.SaveChanges();
                return PatientObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

    }
}
