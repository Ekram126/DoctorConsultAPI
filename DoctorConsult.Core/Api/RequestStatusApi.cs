using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.RequestStatusVM;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace DoctorConsult.Core.Repositories
{
    public class RequestStatusApi : IRequestStatusRepository
    {
        private ApplicationDbContext _context;

        public RequestStatusApi(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds the specified create request model.
        /// </summary>
        /// <param name="createRequestVM">The create request model.</param>
        /// <returns></returns>
        public int Add(RequestStatus createRequestVM)
        {
            RequestStatus reqStatusObj = new RequestStatus();
            try
            {
                if (createRequestVM != null)
                {
                    reqStatusObj.Icon = createRequestVM.Icon;
                    reqStatusObj.Color = createRequestVM.Color;
                    reqStatusObj.Name = createRequestVM.Name;
                    reqStatusObj.NameAr = createRequestVM.NameAr;
                    _context.RequestStatus.Add(reqStatusObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return reqStatusObj.Id;
        }

        public int Delete(int id)
        {
            var reqStatusObj = _context.RequestStatus.Find(id);
            try
            {
                if (reqStatusObj != null)
                {
                    _context.RequestStatus.Remove(reqStatusObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }


        /// <summary>
        /// Get Request Status the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public RequestStatus GetById(int id)
        {
            return _context.RequestStatus.Find(id);
        }

        /// <summary>
        /// Updates the specified edit request status vm.
        /// </summary>
        /// <param name="editRequestVM">The edit request vm.</param>
        /// <returns></returns>
        public int Update(RequestStatus editRequestVM)
        {
            try
            {
                var reqStatusObj = _context.RequestStatus.Find(editRequestVM.Id);
                reqStatusObj.Color = editRequestVM.Color;
                reqStatusObj.Icon = editRequestVM.Icon;
                reqStatusObj.Name = editRequestVM.Name;
                reqStatusObj.NameAr = editRequestVM.NameAr;
                _context.Entry(reqStatusObj).State = EntityState.Modified;
                _context.SaveChanges();
                return reqStatusObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        /// <summary>
        /// Get all Request Status.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IndexRequestStatusVM.GetData> GetAll()
        {
            return _context.RequestStatus.Select(sts => new IndexRequestStatusVM.GetData
            {
                Id = sts.Id,
                Name = sts.Name,
                NameAr = sts.NameAr,
                Color = sts.Color,
                Icon = sts.Icon
            }).ToList();
        }

        /// <summary>
        /// Gets the request status by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public IndexRequestStatusVM GetRequestStatusByUserId(string userId)
        {
            IndexRequestStatusVM mainClass = new IndexRequestStatusVM();
            List<IndexRequestStatusVM.GetData> list = new List<IndexRequestStatusVM.GetData>();
            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            List<string> lstRoleNames = new List<string>();

            List<RequestTracking> lstOpenTracks = new List<RequestTracking>();
            List<RequestTracking> lstCloseTracks = new List<RequestTracking>();
            List<RequestTracking> lstInProgressTracks = new List<RequestTracking>();
            List<RequestTracking> lstSolvedTracks = new List<RequestTracking>();
            List<RequestTracking> lstApprovedTracks = new List<RequestTracking>();


            var roleNames = (from userRole in _context.UserRoles
                             join role in _context.Roles on userRole.RoleId equals role.Id
                             where userRole.UserId == userId
                             select role);
            foreach (var item in roleNames)
            {
                lstRoleNames.Add(item.Name);
            }


            var lstStatus = _context.RequestStatus.ToList();
            mainClass.ListStatus = lstStatus;

            var query = _context.RequestTrackings.Include(a => a.Request)
                                 .Include(a => a.Request.User)
                                 .OrderByDescending(a => a.ResponseDate)
                                 //.Where(a => !string.IsNullOrEmpty(a.AssignTo))
                                 .ToList().GroupBy(rt => rt.RequestId).AsQueryable();



            if (lstRoleNames.Contains("Doctor"))
            {
                query = query.Where(a => a.FirstOrDefault().AssignTo == userId).Where(a => !string.IsNullOrEmpty(a.FirstOrDefault().AssignTo));
            }
            if (lstRoleNames.Contains("Patient"))
            {
                query = query.Where(a => a.FirstOrDefault().Request.CreatedById == userId);
            }
            if (lstRoleNames.Contains("SupervisorDoctor"))
            {
                query = query.Where(a => a.FirstOrDefault().AssignTo == userId).Where(a => !string.IsNullOrEmpty(a.FirstOrDefault().AssignTo));
            }
            if (lstRoleNames.Contains("Admin"))
            {
                query = query;
            }



            if (query.ToList().Count > 0)
            {
                foreach (var req in query.ToList())
                {
                    switch (req.FirstOrDefault().StatusId)
                    {
                        case 1:
                            lstOpenTracks.Add(req.FirstOrDefault());
                            break;
                        case 2:
                            lstInProgressTracks.Add(req.FirstOrDefault());
                            break;
                        case 3:
                            lstSolvedTracks.Add(req.FirstOrDefault());
                            break;
                        case 4:
                            lstApprovedTracks.Add(req.FirstOrDefault());
                            break;
                        case 5:
                            lstCloseTracks.Add(req.FirstOrDefault());
                            break;
                    }

                }
            }

            mainClass.CountOpen = lstOpenTracks.Count;
            mainClass.CountClosed = lstCloseTracks.Count;
            mainClass.CountInProgress = lstInProgressTracks.Count;
            mainClass.CountSolved = lstSolvedTracks.Count;
            mainClass.CountApproved = lstApprovedTracks.Count;
            mainClass.CountAll = query.ToList().Count();

            return mainClass;
        }


        /// <summary>
        /// Gets the request status by user identifier and speciality identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="specialityId">The speciality identifier.</param>
        /// <returns></returns>
        public IndexRequestStatusVM GetRequestStatusByUserIdAndSpecialityId(string userId, int specialityId)
        {
            IndexRequestStatusVM mainClass = new IndexRequestStatusVM();
            List<IndexRequestStatusVM.GetData> list = new List<IndexRequestStatusVM.GetData>();
            ApplicationUser UserObj = new ApplicationUser();
            ApplicationRole roleObj = new ApplicationRole();
            List<string> lstRoleNames = new List<string>();

            List<RequestTracking> lstOpenTracks = new List<RequestTracking>();
            List<RequestTracking> lstCloseTracks = new List<RequestTracking>();
            List<RequestTracking> lstInProgressTracks = new List<RequestTracking>();
            List<RequestTracking> lstSolvedTracks = new List<RequestTracking>();
            List<RequestTracking> lstApprovedTracks = new List<RequestTracking>();


            var roleNames = (from userRole in _context.UserRoles
                             join role in _context.Roles on userRole.RoleId equals role.Id
                             where userRole.UserId == userId
                             select role);
            foreach (var item in roleNames)
            {
                lstRoleNames.Add(item.Name);
            }


            var lstStatus = _context.RequestStatus.ToList();
            mainClass.ListStatus = lstStatus;


            //var query = _context.RequestTrackings.Include(a => a.Request).Include(a => a.RequestStatus).Include(a => a.User).ToList()
            //                        .GroupBy(track => track.RequestId).ToList()
            //                        .Select(g => g.OrderByDescending(track => track.ResponseDate));


            var query = _context.RequestTrackings.Include(a => a.Request)
                                .Include(a => a.RequestStatus).Include(a => a.User)
                                .GroupBy(track => track.RequestId).ToList()
                                .Select(g => g.OrderByDescending(track => track.ResponseDate))
                                .AsQueryable(); // Keep the query deferred


            if (lstRoleNames.Contains("Doctor"))
            {
                query = query.Where(a => a.FirstOrDefault().AssignTo == userId).Where(a => !string.IsNullOrEmpty(a.FirstOrDefault().AssignTo));
            }
            if (lstRoleNames.Contains("Patient"))
            {
                query = query.Where(a => a.FirstOrDefault().Request.CreatedById == userId);
            }
            if (lstRoleNames.Contains("SupervisorDoctor"))
            {
                // query = query.Where(a => a.FirstOrDefault().Request.SpecialityId == specialityId && (a.FirstOrDefault().AssignTo == userId || a.FirstOrDefault().CreatedById == userId)); 

                query = query.Where(g => g.Any(track => track.Request.SpecialityId == specialityId && (track.AssignTo == userId || track.CreatedById == userId)));
            }
            if (lstRoleNames.Contains("Admin"))
            {
                query = query;
            }



            if (query.ToList().Count > 0)
            {
                foreach (var req in query.ToList())
                {
                    switch (req.FirstOrDefault().StatusId)
                    {
                        case 1:
                            lstOpenTracks.Add(req.FirstOrDefault());
                            break;
                        case 2:
                            lstInProgressTracks.Add(req.FirstOrDefault());
                            break;
                        case 3:
                            lstSolvedTracks.Add(req.FirstOrDefault());
                            break;
                        case 4:
                            lstApprovedTracks.Add(req.FirstOrDefault());
                            break;
                        case 5:
                            lstCloseTracks.Add(req.FirstOrDefault());
                            break;
                    }

                }
            }

            mainClass.CountOpen = lstOpenTracks.Count;
            //Pending
            mainClass.CountInProgress = lstInProgressTracks.Count;
            //respond
            mainClass.CountSolved = lstSolvedTracks.Count;

            mainClass.CountApproved = lstApprovedTracks.Count;
            mainClass.CountClosed = lstCloseTracks.Count;
            mainClass.CountAll = query.ToList().Count();

            return mainClass;
        }



    }
}

