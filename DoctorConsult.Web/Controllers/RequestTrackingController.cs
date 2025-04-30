using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.RequestTrackingVM;
using DoctorConsult.ViewModels.UserVM;
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
        IHttpContextAccessor _httpContextAccessor;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public RequestTrackingController(IRequestTrackingRepository requestTrackingService, IRequestRepository requestService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, RoleManager<ApplicationRole> roleManager)
        {
            _requestTrackingService = requestTrackingService;
            _requestService = requestService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            // _roleManager = roleManager;

            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        [HttpPost]
        [Route("AddRequestTracking")]
        public int Post(CreateRequestTrackingVM createRequestTracking)
        {
            return _requestTrackingService.Add(createRequestTracking);
        }


        [HttpPut]
        [Route("UpdateRequestTracking")]
        public int UpdateRequestTracking(EditRequestTrackingVM editRequestTrackingVM)
        {
            return _requestTrackingService.Update(editRequestTrackingVM);
        }




        [HttpPost]
        [Route("SendMailToPatient")]
        public async void SendMailToPatient(CreateRequestTrackingVM createRequestTracking)
        {
            var requestObj = _requestService.GetById(int.Parse(createRequestTracking.RequestId.ToString()));

            var userObj = await _userManager.FindByIdAsync(requestObj.UserId);

            string from = "almostakbaltechnology.dev@gmail.com";
            string subject = "Reply to Patient";
            string appSpecificPassword = "fajtjigwpcnxyyuv";

            var mailMessage2 = new MailMessage(from, userObj.Email, subject, "Please check your account to see doctor reply");
            mailMessage2.IsBodyHtml = true;
            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(from, appSpecificPassword);
                smtpClient.Send(mailMessage2);
            }
        }




        [HttpPost]
        [Route("SendMailToAdminAfterPatientRequest")]
        public async Task<ActionResult> SendMailToAdminAfterPatientRequest()
        {
            //var requestObj = _requestService.GetById(int.Parse(createRequestTracking.RequestId.ToString()));
            //var userObj = await _userManager.FindByIdAsync(requestObj.UserId);

            try
            {
                var role = await _roleManager.FindByNameAsync("Admin");
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

                string from = "almostakbaltechnology.dev@gmail.com";
                string subject = "New Request Added";
                string appSpecificPassword = "fajtjigwpcnxyyuv";
                var domainName = "http://" + _httpContextAccessor.HttpContext?.Request.Host.Value;
                string body = "Hello, Admin: \n\n There is new request added, please click on this link to login to see the request  <a href= '" + domainName + "/#/adminlog'> login </a> ";

                var mailMessage2 = new MailMessage(from, usersInRole[0].Email, subject, body);
                mailMessage2.IsBodyHtml = true;
                using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(from, appSpecificPassword);
                    smtpClient.Send(mailMessage2);
                }
            }
            catch (Exception ex)
            {
                // Log or output the error
                Console.WriteLine($"Error sending email: {ex.Message}");
            }


            return Ok();
        }



        [HttpPost]
        [Route("SendMailToAssignedSupervisorOrDoctor")]
        public async Task<ActionResult> SendMailToAssignedSupervisor(CreateRequestTrackingVM createRequestTracking)
        {
            try
            {
             
                var userObj = await _userManager.FindByIdAsync(createRequestTracking.AssignTo); ;

                StringBuilder builder = new StringBuilder();
                builder.AppendLine("Hello, " + userObj.UserName +"<br/>");
                builder.AppendLine("Please check new requests." + "<br/>");
                var domainName = "http://" + _httpContextAccessor.HttpContext?.Request.Host.Value;
                builder.AppendLine("Please click on this link to login to see the request  <a href= '" + domainName + "/#/adminlog'> login </a> ");


                string from = "almostakbaltechnology.dev@gmail.com";
                string subject = "New Request Added";
                string appSpecificPassword = "fajtjigwpcnxyyuv";
                 string body = builder.ToString();
                
               var mailMessage2 = new MailMessage(from, userObj.Email, subject, body);
                mailMessage2.IsBodyHtml = true;
                using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(from, appSpecificPassword);
                    smtpClient.Send(mailMessage2);
                }
            }
            catch (Exception ex)
            {
                // Log or output the error
                Console.WriteLine($"Error sending email: {ex.Message}");
            }


            return Ok();
        }



        [HttpGet]
        [Route("GetAllTrackingsByRequestId/{reqId}/{userId}")]
        public IndexRequestTrackingVM GetAllTrackingsByRequestId(int reqId, string userId)
        {
            return _requestTrackingService.GetAllTrackingsByRequestId(reqId, userId);
        }

        [HttpGet]
        [Route("GetRequestTrackingById/{trackId}")]
        public EditRequestTrackingVM GetRequestTrackingById(int trackId)
        {
            return _requestTrackingService.GetById(trackId);
        }


    }
}
