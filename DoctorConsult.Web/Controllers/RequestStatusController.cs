using DoctorConsult.Domain.Interfaces;
using DoctorConsult.ViewModels.RequestStatusVM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace  DoctorConsult.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestStatusController : ControllerBase
    {
        private IRequestStatusRepository _requestStatusService;

        public RequestStatusController(IRequestStatusRepository requestStatusService)
        {
            _requestStatusService = requestStatusService;
        }

        //    [HttpGet]
        //    public IEnumerable<IndexRequestStatusVM.GetData> Get()
        //    {
        //        return _requestStatusService.GetAllRequestStatus();
        //    }



        [HttpGet]
        [Route("GetRequestStatusByUserId/{userId}")]
        public IndexRequestStatusVM GetRequestStatusByUserId(string userId)
        {
            return _requestStatusService.GetRequestStatusByUserId(userId);
        }



      [HttpGet]
        [Route("GetRequestStatusByUserIdAndSpecialityId/{userId}/{specialityId}")]
        public IndexRequestStatusVM GetRequestStatusByUserIdAndSpecialityId(string userId, int specialityId)
        {
            return _requestStatusService.GetRequestStatusByUserIdAndSpecialityId(userId,specialityId);
        }



    }
}
