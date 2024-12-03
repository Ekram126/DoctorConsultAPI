using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.RequestTrackingVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoctorConsult.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestTrackingController : ControllerBase
    {
        private IRequestTrackingRepository _requestTrackingService;
        private IRequestRepository _requestService;
        UserManager<ApplicationUser> _userManager;
        public RequestTrackingController(IRequestTrackingRepository requestTrackingService, IRequestRepository requestService, UserManager<ApplicationUser> userManager)
        {
            _requestTrackingService = requestTrackingService;
            _requestService = requestService;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("AddRequestTracking")]
        public int Post(CreateRequestTrackingVM createRequestTracking)
        {
            return _requestTrackingService.Add(createRequestTracking);
        }




        [HttpPost]
        [Route("SendMailToPatient")]
        public async void SendMailToPatient(CreateRequestTrackingVM createRequestTracking)
        {



            var requestObj = _requestService.GetById(int.Parse(createRequestTracking.RequestId.ToString()));

           var userObj = await  _userManager.FindByIdAsync(requestObj.UserId);
     

            StringBuilder strBuild = new StringBuilder();
            //strBuild.Append($"Dear {usr.UserName}\r\n");
            //strBuild.Append("<table>");
            //strBuild.Append("<tr>");
            //strBuild.Append("<td> Asset Name");
            //strBuild.Append("</td>");
            //strBuild.Append("<td>" + masterObj.NameAr);
            //strBuild.Append("</td>");
            //strBuild.Append("</tr>");
            //strBuild.Append("<tr>");
            //strBuild.Append("<td> Serial");
            //strBuild.Append("</td>");
            //strBuild.Append("<td>" + assetObj.SerialNumber);
            //strBuild.Append("</td>");
            //strBuild.Append("</tr>");
            //strBuild.Append("<tr>");
            //strBuild.Append("<td> BarCode");
            //strBuild.Append("</td>");
            //strBuild.Append("<td>" + assetObj.Barcode);
            //strBuild.Append("</td>");
            //strBuild.Append("</tr>");
            //if (applicationObj.AppTypeId == 1)
            //{
            //    strBuild.Append("<tr>");
            //    strBuild.Append("<td> Reasons");
            //    strBuild.Append("</td>");
            //    strBuild.Append("<td>" + strExcludes);
            //    strBuild.Append("</td>");
            //    strBuild.Append("</tr>");
            //}
            //if (applicationObj.AppTypeId == 2)
            //{
            //    strBuild.Append("<tr>");
            //    strBuild.Append("<td> Reasons");
            //    strBuild.Append("</td>");
            //    strBuild.Append("<td>" + strHolds);
            //    strBuild.Append("</td>");
            //    strBuild.Append("</tr>");
            //}
            //strBuild.Append("</table>");




            string from = "almostakbaltechnology.dev@gmail.com";
            string subject = "Reply to Patient";
            string body = strBuild.ToString();
            string appSpecificPassword = "fajtjigwpcnxyyuv";

            var mailMessage2 = new MailMessage(from, userObj.Email, subject, "Please check your email");
            mailMessage2.IsBodyHtml = true;
            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(from, appSpecificPassword);
                smtpClient.Send(mailMessage2);
            }


        }

        [HttpGet]
        [Route("GetAllTrackingsByRequestId/{reqId}")]
        public IndexRequestTrackingVM GetAllTrackingsByRequestId(int reqId)
        {
            return _requestTrackingService.GetAllTrackingsByRequestId(reqId);
        }
    }
}
