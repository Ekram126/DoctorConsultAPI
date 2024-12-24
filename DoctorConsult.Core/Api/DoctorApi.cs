using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.DoctorVM;
using DoctorConsult.ViewModels.RequestVM;
using DoctorConsult.ViewModels.UserVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static Azure.Core.HttpHeader;
using System.Net;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace DoctorConsult.Core.Repositories
{
    public class DoctorApi : IDoctorRepository
    {
        private ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;
        RoleManager<ApplicationRole> _roleManager;

        public DoctorApi(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        ///Craete Doctor
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public int Add(CreateDoctorVM model)
        {

            try
            {
                if (model != null)
                {
                    Doctor DoctorObj = new Doctor();
                    DoctorObj.Code = model.Code;
                    DoctorObj.Name = model.Name;
                    DoctorObj.NameAr = model.NameAr;
                    DoctorObj.NationalId = model.NationalId;
                    DoctorObj.Email = model.Email;
                    DoctorObj.Address = model.Address;
                    DoctorObj.AddressAr = model.AddressAr;
                    DoctorObj.Mobile = model.Mobile;
                    DoctorObj.Dob = model.StrDob != "" ? DateTime.Parse(model.StrDob) : null;
                    DoctorObj.JoinDate = model.StrJoinDate != "" ? DateTime.Parse(model.StrJoinDate) : null;
                    DoctorObj.GradDate = model.StrGradDate != "" ? DateTime.Parse(model.StrGradDate) : null;
                    DoctorObj.GenderId = model.GenderId;
                    DoctorObj.SpecialistId = model.SpecialistId;
                    DoctorObj.ParentId = model.ParentId;
                    DoctorObj.Remarks = model.Remarks;
                    DoctorObj.IsActive = model.IsActive;
                    _context.Doctors.Add(DoctorObj);
                    _context.SaveChanges();
                    return DoctorObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;

        }

        /// <summary>
        /// Checks the doctor role.
        /// </summary>
        /// <param name="doctorUserRoleObj">The doctor user role object.</param>
        /// <returns></returns>
        public async Task<List<IndexUserVM.GetData>> CheckDoctorRole(DoctorUserRole doctorUserRoleObj)
        {
            List<IndexUserVM.GetData> lstUsers = new List<IndexUserVM.GetData>();

            if (doctorUserRoleObj.RoleName == "Doctor")
            {

                var lstDoctors = _context.Doctors.Where(a => a.SpecialistId == doctorUserRoleObj.SpecialityId && a.IsActive == true).ToList();
                if (lstDoctors.Count > 0)
                {
                    var role = await _roleManager.FindByNameAsync("SupervisorDoctor");
                    var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                    var doctorIds = lstDoctors.Select(d => d.Email).ToList(); // Assuming Doctor has a property UserId that links to the user

                    foreach (var item in usersInRole)
                    {
                        if (doctorIds.Contains(item.Email)) // Check if the user is a doctor in the filtered list
                        {
                            IndexUserVM.GetData userObj = new IndexUserVM.GetData();
                            var doctorObj = _context.Doctors.Where(a => a.Email == item.Email).ToList();
                            userObj.Id = item.Id;
                            userObj.DoctorId = doctorObj[0].Id;
                            userObj.UserName = item.UserName;
                            lstUsers.Add(userObj);
                        }
                    }
                }
            }

            return lstUsers;
        }

        /// <summary>
        /// Deletes the specified doctor.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public int Delete(int id)
        {
            var doctorObj = _context.Doctors.Find(id);
            try
            {
                _context.Doctors.Remove(doctorObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        /// <summary>
        /// Gets all doctors
        /// </summary>
        /// <returns></returns>
        public List<IndexDoctorVM.GetData> GetAll()
        {
            List<IndexDoctorVM.GetData> list = new List<IndexDoctorVM.GetData>();
            var lstDoctors = _context.Doctors.Include(a => a.Specialist).Where(a => a.IsActive == true).ToList();
            if (lstDoctors.Count > 0)
            {
                foreach (var item in lstDoctors)
                {
                    IndexDoctorVM.GetData getDataObj = new IndexDoctorVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Code = item.Code;
                    getDataObj.Name = item.Name;
                    getDataObj.NameAr = item.NameAr;
                    getDataObj.NationalId = item.NationalId;
                    getDataObj.Dob = item.Dob;
                    getDataObj.Mobile = item.Mobile;
                    getDataObj.Email = item.Email;
                    getDataObj.Address = item.Address;
                    getDataObj.AddressAr = item.AddressAr;
                    getDataObj.GradDate = item.GradDate;
                    getDataObj.JoinDate = item.JoinDate;
                    getDataObj.Remarks = item.Remarks;
                    getDataObj.GenderId = item.GenderId;
                    getDataObj.SpecialistId = item.SpecialistId;
                    getDataObj.SpecialityName = item.Specialist?.Name;
                    getDataObj.SpecialityNameAr = item.Specialist?.NameAr;
                    getDataObj.DoctorImg = item.DoctorImg;
                    if (item.ParentId == 0)
                    {
                        getDataObj.SupervisorDoctor = item.Name;
                    }
                    else
                    {
                        var child = _context.Doctors.Where(a => a.ParentId == item.ParentId).ToList();
                        if (child.Count > 0)
                        {
                            var s = _context.Doctors.Find(child[0].ParentId);
                            getDataObj.SupervisorDoctor = s.Name;
                        }
                    }

                    list.Add(getDataObj);
                }
            }


            return list;
        }
        /// <summary>
        /// Gets all doctors.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public async Task<IndexDoctorVM> GetAll(SortAndFilterDoctorVM data, int pageNumber, int pageSize)
        {

            IndexDoctorVM mainClass = new IndexDoctorVM();
            List<IndexDoctorVM.GetData> list = new List<IndexDoctorVM.GetData>();
            var lstDoctors = _context.Doctors.Include(a => a.Specialist).ToList();

            List<string> lstRoleNames = new List<string>();
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

            #region Load Data Depend on User
            var doctorEmail = lstDoctors.Where(a => a.Email == userObj.Email).ToList();
            if (doctorEmail.Count() > 0)
            {
                var doctorObj = doctorEmail[0];
                if (lstRoleNames.Contains("SupervisorDoctor"))
                {
                    lstDoctors = lstDoctors.Where(a => a.ParentId == doctorObj.Id).ToList();
                }
                if (lstRoleNames.Contains("Admin"))
                {
                    lstDoctors = lstDoctors;
                }
            }


            #endregion



            //#region Search Criteria
            //if (data.SearchObj?.SpecialistId != 0)
            //{
            //    lstDoctors = lstDoctors.Where(x => x.SpecialistId == data.SearchObj?.SpecialistId).ToList();
            //}
            //#endregion
            #region Sort Criteria

            switch (data.SortObj?.SortBy)
            {
                case "Name":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstDoctors = lstDoctors.OrderBy(x => x.Name).ToList();
                    }
                    else
                    {
                        lstDoctors = lstDoctors.OrderByDescending(x => x.Name).ToList();
                    }
                    break;
                case "الاسم":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstDoctors = lstDoctors.OrderBy(x => x.NameAr).ToList();
                    }
                    else
                    {
                        lstDoctors = lstDoctors.OrderByDescending(x => x.NameAr).ToList();
                    }
                    break;


                case "Speciality":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstDoctors = lstDoctors.OrderBy(x => x.Specialist.Name).ToList();
                    }
                    else
                    {
                        lstDoctors = lstDoctors.OrderByDescending(x => x.Specialist.Name).ToList();
                    }
                    break;
                case "التخصص":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstDoctors = lstDoctors.OrderBy(x => x.Specialist.NameAr).ToList();
                    }
                    else
                    {
                        lstDoctors = lstDoctors.OrderByDescending(x => x.Specialist.NameAr).ToList();
                    }
                    break;





                case "Supervisor Doctor":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstDoctors = lstDoctors.OrderBy(x => x.Name).ToList();
                    }
                    else
                    {
                        lstDoctors = lstDoctors.OrderByDescending(x => x.Name).ToList();
                    }
                    break;
                case "الدكتور المسؤول":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstDoctors = lstDoctors.OrderBy(x => x.NameAr).ToList();
                    }
                    else
                    {
                        lstDoctors = lstDoctors.OrderByDescending(x => x.NameAr).ToList();
                    }
                    break;
                case "Join Date":
                case "تاريخ الالتحاق":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstDoctors = lstDoctors.OrderBy(x => x.JoinDate.Value.Date).ToList();
                    }
                    else
                    {
                        lstDoctors = lstDoctors.OrderByDescending(x => x.JoinDate.Value.Date).ToList();
                    }
                    break;
                case "IsActive":
                case "هل هو مفعل":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstDoctors = lstDoctors.OrderBy(x => x.IsActive).ToList();
                    }
                    else
                    {
                        lstDoctors = lstDoctors.OrderByDescending(x => x.IsActive).ToList();
                    }
                    break;


                case "Mobile":
                case "المحمول":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstDoctors = lstDoctors.OrderBy(x => x.Mobile).ToList();
                    }
                    else
                    {
                        lstDoctors = lstDoctors.OrderByDescending(x => x.Mobile).ToList();
                    }
                    break;



            }

            #endregion

            var countItems = lstDoctors.ToList();
            mainClass.Count = countItems.Count();
            if (pageNumber == 0 && pageSize == 0)
                lstDoctors = lstDoctors.ToList();
            else
                lstDoctors = lstDoctors.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            if (lstDoctors.Count > 0)
            {
                foreach (var item in lstDoctors)
                {
                    IndexDoctorVM.GetData getDataObj = new IndexDoctorVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Code = item.Code;
                    getDataObj.Name = item.Name;
                    getDataObj.NameAr = item.NameAr;
                    getDataObj.NationalId = item.NationalId;
                    getDataObj.Dob = item.Dob;
                    getDataObj.Mobile = item.Mobile;
                    getDataObj.Email = item.Email;
                    getDataObj.Address = item.Address;
                    getDataObj.AddressAr = item.AddressAr;
                    getDataObj.GradDate = item.GradDate;
                    getDataObj.JoinDate = item.JoinDate;
                    getDataObj.Remarks = item.Remarks;
                    getDataObj.GenderId = item.GenderId;
                    getDataObj.SpecialistId = item.SpecialistId;
                    getDataObj.SpecialityName = item.Specialist?.Name;
                    getDataObj.SpecialityNameAr = item.Specialist?.NameAr;
                    getDataObj.IsActive = item.IsActive;
                    getDataObj.DoctorImg = item.DoctorImg;


                    if (item.ParentId == 0)
                    {
                        getDataObj.SupervisorDoctor =  item.Name;
                        getDataObj.DoctorRole = "SupervisorDoctor : ";
                    }
                    else
                    {
                        var child = _context.Doctors.Where(a => a.ParentId == item.ParentId).ToList();
                        if (child.Count > 0)
                        {
                            var s = _context.Doctors.Find(child[0].ParentId);
                            getDataObj.SupervisorDoctor =  s.Name;
                            getDataObj.DoctorRole = "Doctor : ";
                        }
                    }
                    if(item.IsActive == true)
                    {
                        getDataObj.DoctorStatus = "Active";
                    }
                    else
                    {
                        getDataObj.DoctorStatus = "Not Active";
                    }
                    list.Add(getDataObj);
                }
            }
            mainClass.Results = list;
            return mainClass;
        }

        /// <summary>
        /// Gets the doctor by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public EditDoctorVM GetById(int id)
        {
            EditDoctorVM item = new EditDoctorVM();
            var doctors = _context.Doctors.Include(a => a.Specialist).Where(a => a.Id == id).ToList();
            if (doctors.Count > 0)
            {
                item.Id = doctors[0].Id;
                item.Code = doctors[0].Code;
                item.Name = doctors[0].Name;
                item.NameAr = doctors[0].NameAr;
                item.NationalId = doctors[0].NationalId;
                item.Dob = doctors[0].Dob?.Date;
                item.StrDob = doctors[0].Dob.HasValue ? doctors[0].Dob.Value.Date.ToShortDateString() : "";
                item.Mobile = doctors[0].Mobile;
                item.Email = doctors[0].Email;
                item.Address = doctors[0].Address;
                item.AddressAr = doctors[0].AddressAr;
                item.GradDate = doctors[0].GradDate?.Date;
                item.StrGradDate = doctors[0].GradDate.HasValue ? doctors[0].GradDate.Value.Date.ToShortDateString() : "";
                item.JoinDate = doctors[0].JoinDate?.Date;
                item.StrJoinDate = doctors[0].JoinDate.HasValue ? doctors[0].JoinDate.Value.Date.ToShortDateString():"";
                item.Remarks = doctors[0].Remarks;
                item.GenderId = doctors[0].GenderId;
                item.SpecialistId = doctors[0].SpecialistId;
                item.DoctorImg = doctors[0].DoctorImg;
                item.ParentId = doctors[0].ParentId;
                item.SpecialityName = doctors[0].Specialist.Name;
                item.SpecialityNameAr = doctors[0].Specialist.NameAr;
                item.IsActive = doctors[0].IsActive;
            }
            return item;
        }

        /// <summary>
        /// Gets the doctors by speciality identifier.
        /// </summary>
        /// <param name="specialityId">The speciality identifier.</param>
        /// <returns></returns>
        public async Task<List<IndexUserVM.GetData>> GetDoctorsBySpecialityId(int specialityId)
        {
            var lstUsers = new List<IndexUserVM.GetData>();


            var lstDoctors = _context.Doctors.Where(a => a.SpecialistId == specialityId).ToList();
            if (lstDoctors.Count > 0)
            {
                var role = await _roleManager.FindByNameAsync("Doctor");
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                var doctorIds = lstDoctors.Select(d => d.Email).ToList(); // Assuming Doctor has a property UserId that links to the user

                foreach (var item in usersInRole)
                {
                    if (doctorIds.Contains(item.Email)) // Check if the user is a doctor in the filtered list
                    {
                        IndexUserVM.GetData userObj = new IndexUserVM.GetData();
                        var doctorObj = _context.Doctors.Where(a => a.Email == item.Email).ToList();
                        userObj.Id = item.Id;
                        userObj.DoctorId = doctorObj[0].Id;
                        userObj.UserName = item.UserName;
                        lstUsers.Add(userObj);
                    }
                }
            }


            return lstUsers;
        }

        /// <summary>
        /// Updates the doctor model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public int Update(EditDoctorVM model)
        {
            try
            {
                var DoctorObj = _context.Doctors.Find(model.Id);
                DoctorObj.Id = model.Id;
                DoctorObj.Code = model.Code;
                DoctorObj.Name = model.Name;
                DoctorObj.NameAr = model.NameAr;
                DoctorObj.NationalId = model.NationalId;
                if (model.StrDob != "")
                    DoctorObj.Dob = DateTime.Parse(model.StrDob.ToString());
                DoctorObj.Mobile = model.Mobile;
                DoctorObj.Email = model.Email;
                DoctorObj.Address = model.Address;
                DoctorObj.AddressAr = model.AddressAr;
                if (model.StrGradDate != "")
                    DoctorObj.GradDate = DateTime.Parse(model.StrGradDate.ToString());

                if (model.StrJoinDate != "")
                    DoctorObj.JoinDate = DateTime.Parse(model.StrJoinDate.ToString());


                DoctorObj.Remarks = model.Remarks;
                DoctorObj.GenderId = model.GenderId;
                DoctorObj.SpecialistId = model.SpecialistId;
                DoctorObj.IsActive = model.IsActive;
                DoctorObj.DoctorImg = model.DoctorImg;
                DoctorObj.ParentId = model.ParentId;
                _context.Entry(DoctorObj).State = EntityState.Modified;
                _context.SaveChanges();
                return DoctorObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        /// <summary>
        /// Updates the doctor image after insert.
        /// </summary>
        /// <param name="modelObj">The model object.</param>
        /// <returns></returns>
        public int UpdateDoctorImageAfterInsert(CreateDoctorVM modelObj)
        {
            var doctorObj = _context.Doctors.Find(modelObj.Id);
            doctorObj.DoctorImg = modelObj.DoctorImg;
            _context.Entry(doctorObj).State = EntityState.Modified;
            _context.SaveChanges();
            return doctorObj.Id;
        }



        /// <summary>
        /// Generates the doctor code.
        /// </summary>
        /// <returns></returns>
        public GeneratedDoctorCodeVM GenerateDoctorCode()
        {
            GeneratedDoctorCodeVM doctorObj = new GeneratedDoctorCodeVM();
            string doctorCode = "DT";

            var lstIds = _context.Doctors.ToList();
            if (lstIds.Count > 0)
            {
                var code = lstIds.LastOrDefault().Id;
                doctorObj.Code = doctorCode + (code + 1);
            }
            else
            {
                doctorObj.Code = doctorCode + 1;
            }

            return doctorObj;
        }

     
    }
}
