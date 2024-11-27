using Microsoft.AspNetCore.Mvc;
using DoctorConsult.ViewModels.RequestVM;
using DoctorConsult.Domain.Interfaces;
using System.Net;
using System.Net.Http.Headers;

namespace DoctorConsult.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepository _requestService;

        IWebHostEnvironment _webHostingEnvironment;

        public RequestController(IRequestRepository requestService, IWebHostEnvironment webHostingEnvironment)
        {
            _requestService = requestService;
            _webHostingEnvironment = webHostingEnvironment;
        }




        [HttpPost]
        [Route("ListRequests/{pageNumber}/{pageSize}")]
        public async Task<IndexRequestVM> ListRequests(SortAndFilterRequestVM data, int pageNumber, int pageSize)
       {
            return await _requestService.ListRequests(data, pageNumber, pageSize);
        }

        [HttpGet]
        [Route("GenerateRequestNumber")]
        public GeneratedRequestNumberVM GenerateRequestNumber()
        {
            return _requestService.GenerateRequestNumber();
        }

        [HttpPost]
        [Route("AddRequest")]
        public int AddRequest(CreateRequestVM createRequestVM)
        {
            var requestId = _requestService.Add(createRequestVM);
            return requestId;
        }

        [HttpGet]
        [Route("GetRequestById/{requestId}")]
        public ViewRequestVM AddRequest(int requestId)
        {
            var requestObj = _requestService.GetById(requestId);
            return requestObj;
        }




    }
}
