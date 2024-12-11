using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.RequestTrackingVM;
using Microsoft.EntityFrameworkCore;

namespace DoctorConsult.Core.Repositories
{
    public class RequestTrackingApi : IRequestTrackingRepository
    {
        private readonly ApplicationDbContext _context;
        private string? msg;

        public RequestTrackingApi(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(CreateRequestTrackingVM createRequestTracking)
        {
            try
            {
                if (createRequestTracking != null)
                {
                    RequestTracking requestTracking = new RequestTracking();
                    requestTracking.Advice = createRequestTracking.Advice;

                    if (createRequestTracking.StrRespondDate != "")
                        requestTracking.ResponseDate = DateTime.Parse(createRequestTracking.StrRespondDate); //requestDescriptionDTO.DescriptionDate;
                    else
                    {
                        DateTime convertedDate = DateTime.Now;


                        DateTime utcTime = convertedDate.ToUniversalTime(); // convert it to Utc using timezone setting of server computer
                        TimeZoneInfo egypt = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                        DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, egypt); // convert from utc to local
                        requestTracking.ResponseDate = localTime.ToLocalTime();


                        //requestTracking.ResponseDate =  convertedDate.ToLocalTime();
                    }
                    requestTracking.RequestId = int.Parse(createRequestTracking.RequestId.ToString());
                    requestTracking.StatusId = createRequestTracking.StatusId;
                    requestTracking.CreatedById = createRequestTracking.CreatedById;
                    requestTracking.AssignTo = createRequestTracking.AssignTo;
                    _context.RequestTrackings.Add(requestTracking);
                    _context.SaveChanges();
                    createRequestTracking.Id = requestTracking.Id;
                    return createRequestTracking.Id;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return createRequestTracking.Id;

        }
        public void Delete(int id)
        {
            RequestTracking requestTracking = _context.RequestTrackings.Find(id);
            try
            {
                if (requestTracking != null)
                {
                    _context.RequestTrackings.Remove(requestTracking);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public IndexRequestTrackingVM GetAllTrackingsByRequestId(int RequestId)
        {
            IndexRequestTrackingVM mainClass = new IndexRequestTrackingVM();


            var trackings = _context.RequestTrackings.Include(a => a.Request).Include(a => a.Request.User).Include(a => a.RequestStatus)
                   .Where(r => r.RequestId == RequestId).OrderByDescending(t => t.ResponseDate).Select(track => new IndexRequestTrackingVM.GetData
                   {
                       Id = track.Id,

                       Advice = track.Advice,
                       ResponseDate = track.ResponseDate,
                       CreatedByName = track.Request.User != null ? track.User.UserName : "",
                       AssignedToUser = track.Request.User != null ? track.AssignToUser.UserName : "",
                       StatusId = track.StatusId != null ? (int)track.StatusId : 0,
                       StatusName = track.RequestStatus != null ? track.RequestStatus.Name : "",
                       StatusNameAr = track.RequestStatus != null ? track.RequestStatus.NameAr : "",
                       StatusColor = track.RequestStatus != null ? track.RequestStatus.Color : "",
                       StatusIcon = track.RequestStatus != null ? track.RequestStatus.Icon : "",
                   }).ToList();


            mainClass.Results = trackings;
            mainClass.Count = trackings.Count;

            return mainClass;
        }

        public IndexRequestTrackingVM.GetData GetById(int id)
        {
            var RequestTrackingObj = _context.RequestTrackings.Include(a => a.Request)

                .Select(track => new IndexRequestTrackingVM.GetData
                {
                    Id = track.Id,
                    Advice = track.Advice,
                    ResponseDate = track.ResponseDate,
                    //CreatedById = track.CreatedById,
                    //DoctorName = track.Doctor.Name,
                    //ManagerDoctorName = track.Doctor.NameAr,
                    StatusId = track.StatusId != null ? (int)track.StatusId : 0,
                    StatusName = track.RequestStatus != null ? track.RequestStatus.Name : "",
                    StatusNameAr = track.RequestStatus != null ? track.RequestStatus.NameAr : "",
                    StatusColor = track.RequestStatus != null ? track.RequestStatus.Color : "",
                    StatusIcon = track.RequestStatus != null ? track.RequestStatus.Icon : "",


                }).FirstOrDefault();
            return RequestTrackingObj;
        }

        public void Update(EditRequestTrackingVM editRequestTracking)
        {
            try
            {
                RequestTracking requestTracking = _context.RequestTrackings.Find(editRequestTracking.Id);
                requestTracking.Advice = editRequestTracking.Advice;
                requestTracking.ResponseDate = DateTime.Now;
                _context.Entry(requestTracking).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }


        public RequestTracking GetFirstTrackForRequestByRequestId(int requestId)
        {
            RequestTracking trackingObj = new RequestTracking();
            var lstTracks = _context.RequestTrackings.Where(r => r.RequestId == requestId && r.StatusId == 1).OrderBy(a => a.Id).ToList();
            if (lstTracks.Count > 0)
            {
                trackingObj = lstTracks[0];
            }

            return trackingObj;
        }

        public RequestTracking GetLastTrackForRequestByRequestId(int requestId)
        {
            RequestTracking trackingObj = new RequestTracking();
            var lstTracks = _context.RequestTrackings.Where(r => r.RequestId == requestId).OrderByDescending(a => a.Id).ToList();
            if (lstTracks.Count > 0)
            {
                trackingObj = lstTracks[0];
            }

            return trackingObj;
        }
    }
}






