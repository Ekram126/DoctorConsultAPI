using DoctorConsult.Domain.Interfaces;
using DoctorConsult.ViewModels.RequestTrackingVM;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoctorConsult.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestTrackingController : ControllerBase
    {
        private IRequestTrackingRepository _requestTrackingService;

        public RequestTrackingController(IRequestTrackingRepository requestTrackingService)
        {
            _requestTrackingService = requestTrackingService;
        }

        [HttpPost]
        [Route("AddRequestTracking")]
        public int Post(CreateRequestTrackingVM createRequestTracking)
        {
            return _requestTrackingService.Add(createRequestTracking);
        }


        [HttpGet]
        [Route("GetAllTrackingsByRequestId/{reqId}")]
        public IndexRequestTrackingVM GetAllTrackingsByRequestId(int reqId)
        {
            return _requestTrackingService.GetAllTrackingsByRequestId(reqId);
        }
    }
}
