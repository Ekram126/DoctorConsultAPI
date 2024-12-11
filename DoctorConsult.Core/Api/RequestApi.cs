using Azure.Core;
using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.RequestDocumentVM;
using DoctorConsult.ViewModels.RequestVM;
using DoctorConsult.ViewModels.UserVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Request = DoctorConsult.Models.Request;

namespace DoctorConsult.Core.Repositories
{
    public class RequestApi : IRequestRepository
    {
        UserManager<ApplicationUser> _userManager;
        RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public RequestApi(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        /// <summary>
        /// Gets all requests.
        /// </summary>
        /// <returns></returns>
        private IQueryable<RequestTracking?>? GetAllRequests()
        {
            //var lastStatusPerRequest = _context.RequestTrackings
            //                            .Include(a => a.Request)
            //                            .Include(r => r.Request.Specialist)
            //                            .Include(a => a.RequestStatus)
            //                            .Include(a => a.User)
            //                            .ToList()
            //                            .GroupBy(track => track.RequestId)
            //                            .Select(g => g.OrderByDescending(track => track.Request.RequestDate).FirstOrDefault());


            var lastStatusPerRequest = _context.RequestTrackings
                                 .Include(a => a.Request)
                                 .Include(r => r.Request.Specialist)
                                 .Include(a => a.RequestStatus)
                                 .Include(a => a.User)
                                 .ToList()
                                 .GroupBy(track => track.RequestId)
                                 .Select(g => g.OrderByDescending(track => track.ResponseDate).FirstOrDefault());


            return lastStatusPerRequest.AsQueryable();
        }

        /// <summary>
        /// Lists the requests.
        /// </summary>
        /// <param name="data">The data. Sort and filter</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public async Task<IndexRequestVM> ListRequests(SortAndFilterRequestVM data, int pageNumber, int pageSize)
        {
            #region Initial Variables
            IQueryable<RequestTracking?>? query = GetAllRequests();

            IndexRequestVM mainClass = new IndexRequestVM();
            List<IndexRequestVM.GetData> list = new List<IndexRequestVM.GetData>();

            List<string> lstRoleNames = new List<string>();
            Doctor doctorObj = new Doctor();
            Patient patientObj = new Patient();
            var userObj = new ApplicationUser();
            #endregion

            //#region User Role

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
            //#endregion

            #region Load Data Depend on User
            if (lstRoleNames.Contains("Doctor"))
            {
                query = query.Where(a => a.AssignTo == userObj.Id || a.CreatedById == userObj.Id);
            }
            if (lstRoleNames.Contains("Patient"))
            {
                query = query.Where(a => a.Request.CreatedById == userObj.Id);
            }
            if (lstRoleNames.Contains("SupervisorDoctor"))
            {
                query = query.Where(a => a.Request.SpecialityId == data.SearchObj.SpecialityId);
            }
            if (lstRoleNames.Contains("Admin"))
            {
                query = query;
            }



            #endregion

            #region Search Criteria
            if (data.SearchObj.StatusId != 0)
            {
                query = query.Where(x => x.StatusId == data.SearchObj.StatusId);
            }

            #endregion

            #region Sort Criteria
            switch (data.SortObj.SortBy)
            {
                case "Subject":
                case "الموضوع":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.Subject);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.Subject);
                    }
                    break;

                case "Request Code":
                case "رقم الطلب":

                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.RequestCode);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.RequestCode);
                    }
                    break;




                case "Speciality":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.Specialist.Name);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.Specialist.Name);
                    }
                    break;
                case "التخصص":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.Specialist.NameAr);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.Specialist.NameAr);
                    }
                    break;


                case "Patient":
                case "المريض":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.User.UserName);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.User.UserName);
                    }
                    break;


                case "Status":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.RequestStatus.Name);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.RequestStatus.Name);
                    }
                    break;


                case "الحالة":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.RequestStatus.NameAr);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.RequestStatus.NameAr);
                    }
                    break;

                case "Date":
                case "التاريخ":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.Request.RequestDate);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Request.RequestDate);
                    }
                    break;


                case "CreatedBy":
                case "تم بواسطة":
                    if (data.SortObj.SortStatus == "ascending")
                    {
                        query = query.OrderBy(x => x.User.UserName);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.User.UserName);
                    }
                    break;
            }
            #endregion

            query = query.OrderByDescending(a => a.ResponseDate.HasValue ? a.ResponseDate : DateTime.MinValue);



            #region Count data and fiter By Paging
            IQueryable<RequestTracking> lstResults;
            var countItems = query.ToList();
            mainClass.Count = countItems.Count();
            if (pageNumber == 0 && pageSize == 0)
                lstResults = query;
            else
                lstResults = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            #endregion

            #region Loop to get Items after serach and sort

            foreach (var req in lstResults.ToList())
            {
                IndexRequestVM.GetData getDataObj = new IndexRequestVM.GetData();
                getDataObj.Id = req.Request.Id;
                getDataObj.RequestCode = req.Request != null ? req.Request.RequestCode : "";
                getDataObj.Subject = req.Request.Subject;
                getDataObj.RequestDate = req.Request.RequestDate;
              
                var lstTracks = _context.RequestTrackings.Include(a => a.RequestStatus).Where(a => a.RequestId == req.Request.Id).OrderByDescending(a => a.ResponseDate).ToList();
                if (lstTracks.Count > 0)
                {
                    var trackObj = lstTracks[0];
                    getDataObj.StatusId = trackObj.RequestStatus != null ? (int)trackObj.RequestStatus.Id : 0;
                    getDataObj.StatusName = trackObj.RequestStatus != null ? trackObj.RequestStatus.Name : "";
                    getDataObj.StatusNameAr = trackObj.RequestStatus != null ? trackObj.RequestStatus.NameAr : "";
                    getDataObj.StatusColor = trackObj.RequestStatus != null ? trackObj.RequestStatus.Color : "";
                    getDataObj.StatusIcon = trackObj.RequestStatus != null ? trackObj.RequestStatus.Icon : "";


                    getDataObj.ActionDate = trackObj.ResponseDate;
                }
                getDataObj.Advice = req.Advice;
                getDataObj.CreatedBy = req.Request.User != null ? req.Request.User.UserName : "";
                getDataObj.Complain = req.Request.Complain;
                getDataObj.SpecialityName = req.Request.Specialist != null ? req.Request.Specialist.Name : "";
                getDataObj.SpecialityNameAr = req.Request.Specialist != null ? req.Request.Specialist.NameAr : "";
                list.Add(getDataObj);
            }
            #endregion

            #region Represent data after Paging and count
            mainClass.Results = list;
            #endregion

            return mainClass;
        }
        /// <summary>
        /// Generates the request number.
        /// </summary>
        /// <returns></returns>
        public GeneratedRequestNumberVM GenerateRequestNumber()
        {
            GeneratedRequestNumberVM numberObj = new GeneratedRequestNumberVM();
            string WO = "Req";

            var lstIds = _context.Requests.ToList();
            if (lstIds.Count > 0)
            {
                var code = lstIds.LastOrDefault().Id;
                numberObj.RequestCode = WO + (code + 1);
            }
            else
            {
                numberObj.RequestCode = WO + 1;
            }

            return numberObj;
        }
        /// <summary>
        /// Adds the specified create request model.
        /// </summary>
        /// <param name="createRequestVM">The create request model.</param>
        /// <returns></returns>
        public int Add(CreateRequestVM createRequestVM)
        {
            try
            {
                if (createRequestVM != null)
                {
                    Request request = new Request();
                    request.Subject = createRequestVM.Subject;
                    request.RequestCode = createRequestVM.RequestCode;
                    request.RequestDate = DateTime.Parse(createRequestVM.StrRequestDate.ToString());// DateTime.Now; 
                    request.Complain = createRequestVM.Complain;
                    request.CreatedById = createRequestVM.CreatedById;
                    request.SpecialityId = createRequestVM.SpecialityId;
                    _context.Requests.Add(request);
                    _context.SaveChanges();
                    createRequestVM.Id = request.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return createRequestVM.Id;
        }

        /// <summary>
        /// Updates the specified edit request model.
        /// </summary>
        /// <param name="editRequestVM">The edit request model.</param>
        public void Update(EditRequestVM editRequestVM)
        {

            try
            {
                Request request = _context.Requests.Find(editRequestVM.Id);
                request.Id = editRequestVM.Id;
                request.Subject = editRequestVM.Subject;
                request.RequestCode = editRequestVM.RequestCode;
                request.RequestDate = editRequestVM.RequestDate;
                request.CreatedById = editRequestVM.CreatedById;
                _context.Entry(request).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }



        /// <summary>
        /// Deletes the specified Request By Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Delete(int id)
        {
            Request request = _context.Requests.Find(id);
            try
            {
                if (request != null)
                {

                    _context.Requests.Remove(request);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        /// <summary>
        /// Get REquest the by Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ViewRequestVM GetById(int id)
        {
            ViewRequestVM requestDTO = new ViewRequestVM();
            var lstRequests = _context.Requests.Include(r => r.Specialist)
               .Include(r => r.User).Where(e => e.Id == id).ToList();


            if (lstRequests.Count > 0)
            {
                Request req = lstRequests[0];
                requestDTO.Id = req.Id;
                requestDTO.Subject = req.Subject;
                requestDTO.RequestCode = req.RequestCode;
                requestDTO.RequestDate = req.RequestDate;
                requestDTO.UserName = req.User?.UserName;
                requestDTO.UserId = req.User?.Id;
                requestDTO.Complain = req.Complain;
                requestDTO.SpecialityName = req.Specialist != null ? req.Specialist.Name : "";
                requestDTO.SpecialityNameAr = req.Specialist != null ? req.Specialist.NameAr:"";
                requestDTO.SpecialityId = int.Parse(req.SpecialityId.ToString());


               var  trackStatus = _context.RequestTrackings.Where(a => a.RequestId == id).OrderByDescending(a => a.ResponseDate).ToList();
                if (trackStatus.Count > 0)
                {
                    requestDTO.StatusId = trackStatus[0].StatusId;
                }



                var lstTracks = _context.RequestTrackings.Where(a => a.RequestId == id).ToList();
                if (lstTracks.Count > 0)
                {
                    var firstTrack = lstTracks.FirstOrDefault();
                    if (firstTrack != null)
                    {
                        var docs = _context.RequestDocuments.Where(a => a.RequestTrackingId == firstTrack.Id).ToList().Select(item => new IndexRequestDocumentVM
                        {
                            FileName = item.FileName,
                            Title = item.Title,
                            Id = item.Id,
                            RequestTrackingId = item.RequestTrackingId
                        });

                        requestDTO.listDocuments = docs.ToList();
                    }
                }
            }
            return requestDTO;
        }







    }
}