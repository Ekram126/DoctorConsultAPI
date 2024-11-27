using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.RequestStatusVM;
using Microsoft.EntityFrameworkCore;


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



        public RequestStatus GetById(int id)
        {
            return _context.RequestStatus.Find(id);
        }

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
                            lstCloseTracks.Add(req.FirstOrDefault());
                            break;
                        case 3:
                            lstInProgressTracks.Add(req.FirstOrDefault());
                            break;
                        case 4:
                            lstSolvedTracks.Add(req.FirstOrDefault());
                            break;
                        case 5:
                            lstApprovedTracks.Add(req.FirstOrDefault());
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

            //var query = _context.RequestTrackings.Include(a => a.Request)
            //                     .Include(a => a.Request.User)
            //                     .OrderByDescending(a => a.ResponseDate)
            //                   //    .Where(a => !string.IsNullOrEmpty(a.AssignTo))
            //                     .ToList().GroupBy(rt => rt.RequestId).AsQueryable();




            var query = _context.RequestTrackings
                                        .Include(a => a.Request)
                                        .ThenInclude(r => r.User) // Ensure User is included if AssignTo is a foreign key
                                        .Include(a => a.Request)
                                        .ThenInclude(r => r.Specialist)
                                        .Include(a => a.RequestStatus)
                                        .Include(a => a.User) // Include User if necessary
                                      //  .Where(a => !string.IsNullOrEmpty(a.AssignTo))// Ensure AssignTo is not null or empty
                                        .OrderByDescending(a => a.ResponseDate)
                                        .ToList() 
                                        .GroupBy(track => track.RequestId).AsQueryable();











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
                query = query.Where(a => a.FirstOrDefault().Request.SpecialityId == specialityId);
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
                            lstCloseTracks.Add(req.FirstOrDefault());
                            break;
                        case 3:
                            lstInProgressTracks.Add(req.FirstOrDefault());
                            break;
                        case 4:
                            lstSolvedTracks.Add(req.FirstOrDefault());
                            break;
                        case 5:
                            lstApprovedTracks.Add(req.FirstOrDefault());
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

    }
}

