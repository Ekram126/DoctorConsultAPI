using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.RequestDocumentVM;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;

namespace DoctorConsult.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestDocumentController : ControllerBase
    {
        private IRequestDocumentRepository _requestDocumentService;
        IWebHostEnvironment _webHostingEnvironment;
        public RequestDocumentController(IRequestDocumentRepository requestDocumentService, IWebHostEnvironment webHostingEnvironment)
        {
            _requestDocumentService = requestDocumentService;
            _webHostingEnvironment = webHostingEnvironment;
        }



        [HttpPost]
        [Route("CreateRequestDocuments")]
        public void CreateRequestDocuments(CreateRequestDocumentVM requestDocuments)
        {
            _requestDocumentService.Add(requestDocuments);
        }
        [HttpPost]
        [Route("UploadRequestFiles")]
        public ActionResult UploadRequestFiles(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/RequestDocuments";
            bool exists = System.IO.Directory.Exists(folderPath);
            if (!exists)
                System.IO.Directory.CreateDirectory(folderPath);

            string filePath = folderPath + "/" + file.FileName;
            if (System.IO.File.Exists(filePath))
            {

            }
            else
            {
                Stream stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
                stream.Close();
            }
            return StatusCode(StatusCodes.Status201Created);
        }


        [Route("GetRequestDocumentsByRequestTrackingId/{trackId}")]
        public IEnumerable<IndexRequestDocumentVM> GetRequestDocumentsByRequestTrackingId(int trackId)
        {
            return _requestDocumentService.GetRequestDocumentsByRequestTrackingId(trackId);
        }
    }
}
