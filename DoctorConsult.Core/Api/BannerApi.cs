using BannerConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.BannerVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;



namespace BannerConsult.Core.Repositories
{

    public class BannerApi : IBannerRepository
    {
        private ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;
        RoleManager<ApplicationRole> _roleManager;

        public BannerApi(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>Add Banner</summary>
        /// <param name="model"></param>
        public int Add(CreateBannerVM model)
        {

            try
            {
                if (model != null)
                {
                    Banner bannerObj = new Banner();
                    bannerObj.Code = model.Code;
                    bannerObj.Name = model.Name;
                    bannerObj.NameAr = model.NameAr;
                    bannerObj.Brief = model.Brief;
                    bannerObj.BriefAr = model.BriefAr;


                    // Get the current UTC time
                    DateTime utcTime = DateTime.UtcNow;
                    // Time zone ID for Egypt
                    string egyptTimeZoneId = "E. Europe Standard Time";
                    // Convert UTC time to Egypt Time
                    TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById(egyptTimeZoneId);
                    DateTime egyptTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, egyptTimeZone);
                    bannerObj.BannerDate = egyptTime;


                    bannerObj.BannerImg = model.BannerImg;
                    bannerObj.SpecialistId = model.SpecialistId;
                    bannerObj.OrderId = model.OrderId;
                    bannerObj.IsActive = model.IsActive;



                    _context.Banners.Add(bannerObj);
                    _context.SaveChanges();
                    return bannerObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;

        }

        /// <summary>
        /// Delete Banner By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id)
        {
            var BannerObj = _context.Banners.Find(id);
            try
            {
                _context.Banners.Remove(BannerObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        /// <summary>
        /// Get All Banners
        /// </summary>
        /// <returns></returns>
        public List<IndexBannerVM.GetData> GetAll()
        {
            List<IndexBannerVM.GetData> list = new List<IndexBannerVM.GetData>();
            var lstBanners = _context.Banners.Include(a => a.Specialist).Where(a => a.IsActive == true).ToList();
            if (lstBanners.Count > 0)
            {
                foreach (var item in lstBanners)
                {
                    IndexBannerVM.GetData getDataObj = new IndexBannerVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Code = item.Code;
                    getDataObj.Name = item.Name;
                    getDataObj.NameAr = item.NameAr;

                    getDataObj.SpecialistId = item.SpecialistId;
                    getDataObj.Brief = item.Brief;
                    getDataObj.BriefAr = item.BriefAr;
                    getDataObj.IsActive = item.IsActive;
                    getDataObj.BannerImg = item.BannerImg;
                    getDataObj.OrderId = item.OrderId;
                    getDataObj.BannerDate = item.BannerDate.Value.ToShortDateString();

                    list.Add(getDataObj);
                }
            }


            return list;
        }




        /// <summary>
        /// List All Banners with sort and Search with pageNumber and PageSize
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IndexBannerVM> GetAll(SortAndFilterBannerVM data, int pageNumber, int pageSize)
        {

            IndexBannerVM mainClass = new IndexBannerVM();
            List<IndexBannerVM.GetData> list = new List<IndexBannerVM.GetData>();
            var lstBanners = _context.Banners.Include(a => a.Specialist).ToList();

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

            //if (lstRoleNames.Contains("SupervisorBanner"))
            //{
            //    lstBanners = lstBanners.Where(a => a.Request.CreatedById == userObj.Id);
            //}
            //if (lstRoleNames.Contains("Admin"))
            //{
            //    lstBanners = lstBanners;
            //}

            #endregion



            //#region Search Criteria
            //if (data.SearchObj?.SpecialistId != 0)
            //{
            //    lstBanners = lstBanners.Where(x => x.SpecialistId == data.SearchObj?.SpecialistId).ToList();
            //}
            //#endregion
            #region Sort Criteria

            switch (data.SortObj?.SortBy)
            {
                case "Name":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstBanners = lstBanners.OrderBy(x => x.Name).ToList();
                    }
                    else
                    {
                        lstBanners = lstBanners.OrderByDescending(x => x.Name).ToList();
                    }
                    break;
                case "الاسم":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstBanners = lstBanners.OrderBy(x => x.NameAr).ToList();
                    }
                    else
                    {
                        lstBanners = lstBanners.OrderByDescending(x => x.NameAr).ToList();
                    }
                    break;


                case "Speciality":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstBanners = lstBanners.OrderBy(x => x.Specialist.Name).ToList();
                    }
                    else
                    {
                        lstBanners = lstBanners.OrderByDescending(x => x.Specialist.Name).ToList();
                    }
                    break;
                case "التخصص":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstBanners = lstBanners.OrderBy(x => x.Specialist.NameAr).ToList();
                    }
                    else
                    {
                        lstBanners = lstBanners.OrderByDescending(x => x.Specialist.NameAr).ToList();
                    }
                    break;





                case "Supervisor Banner":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstBanners = lstBanners.OrderBy(x => x.Name).ToList();
                    }
                    else
                    {
                        lstBanners = lstBanners.OrderByDescending(x => x.Name).ToList();
                    }
                    break;
                case "الدكتور المسؤول":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstBanners = lstBanners.OrderBy(x => x.NameAr).ToList();
                    }
                    else
                    {
                        lstBanners = lstBanners.OrderByDescending(x => x.NameAr).ToList();
                    }
                    break;









                case "Banner Date":
                case "التاريخ":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstBanners = lstBanners.OrderBy(x => x.BannerDate.Value.Date).ToList();
                    }
                    else
                    {
                        lstBanners = lstBanners.OrderByDescending(x => x.BannerDate.Value.Date).ToList();
                    }
                    break;
                case "IsActive":
                case "هل هو مفعل":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstBanners = lstBanners.OrderBy(x => x.IsActive).ToList();
                    }
                    else
                    {
                        lstBanners = lstBanners.OrderByDescending(x => x.IsActive).ToList();
                    }
                    break;




            }

            #endregion

            var countItems = lstBanners.ToList();
            mainClass.Count = countItems.Count();
            if (pageNumber == 0 && pageSize == 0)
                lstBanners = lstBanners.ToList();
            else
                lstBanners = lstBanners.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            if (lstBanners.Count > 0)
            {
                foreach (var item in lstBanners)
                {
                    IndexBannerVM.GetData getDataObj = new IndexBannerVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Code = item.Code;
                    getDataObj.Name = item.Name;
                    getDataObj.NameAr = item.NameAr;

                    getDataObj.SpecialistId = item.SpecialistId;
                    getDataObj.Brief = item.Brief;
                    getDataObj.BriefAr = item.BriefAr;
                    getDataObj.IsActive = item.IsActive;
                    getDataObj.BannerImg = item.BannerImg;
                    getDataObj.OrderId = item.OrderId;
                    getDataObj.BannerDate = item.BannerDate.Value.ToShortDateString();

                    list.Add(getDataObj);
                }
            }
            mainClass.Results = list;
            return mainClass;
        }



        /// <summary>
        /// Get Banner By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EditBannerVM GetById(int id)
        {
            return _context.Banners.Include(a => a.Specialist).Where(a => a.Id == id).Select(item => new EditBannerVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr,
                BannerDate= item.BannerDate,
                SpecialistId = item.SpecialistId,
                BannerImg = item.BannerImg,
                Brief = item.Brief,
                BriefAr = item.BriefAr,
                IsActive = item.IsActive,
                OrderId = item.OrderId
            }).First();
        }


        /// <summary>
        /// Update Banner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(EditBannerVM model)
        {
            try
            {
                var bannerObj = _context.Banners.Find(model.Id);
                bannerObj.Code = model.Code;
                bannerObj.Name = model.Name;
                bannerObj.NameAr = model.NameAr;
                bannerObj.Brief = model.Brief;
                bannerObj.BriefAr = model.BriefAr;


                // Get the current UTC time
                DateTime utcTime = DateTime.UtcNow;
                // Time zone ID for Egypt
                string egyptTimeZoneId = "E. Europe Standard Time";
                // Convert UTC time to Egypt Time
                TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById(egyptTimeZoneId);
                DateTime egyptTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, egyptTimeZone);
                bannerObj.BannerDate = egyptTime;


                bannerObj.BannerImg = model.BannerImg;
                bannerObj.SpecialistId = model.SpecialistId;
                bannerObj.OrderId = model.OrderId;
                bannerObj.IsActive = model.IsActive;


                _context.Entry(bannerObj).State = EntityState.Modified;
                _context.SaveChanges();
                return bannerObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        /// <summary>
        /// Update BannerImage After Insert
        /// </summary>
        /// <param name="modelObj"></param>
        /// <returns></returns>
        public int UpdateBannerImageAfterInsert(CreateBannerVM modelObj)
        {
            var BannerObj = _context.Banners.Find(modelObj.Id);
            BannerObj.BannerImg = modelObj.BannerImg;
            _context.Entry(BannerObj).State = EntityState.Modified;
            _context.SaveChanges();
            return BannerObj.Id;
        }


        
    }
}
