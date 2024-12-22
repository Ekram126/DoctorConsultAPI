using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.UserVM;
using DoctorConsult.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DoctorConsult.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        RoleManager<ApplicationRole> _roleManager;
        // PasswordValidatorService _passwordValidatorService;
        private readonly IPasswordValidator<ApplicationUser> _passwordValidatorService;



        private readonly IConfiguration _configuration;

        private readonly ApplicationDbContext _context;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
          IConfiguration configuration, IPasswordValidator<ApplicationUser> passwordValidatorService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _passwordValidatorService = passwordValidatorService;


        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginVM userObj)
        {
            string Useremail = "";
            string userName = "";
            var user = await _userManager.FindByNameAsync(userObj.Username);

            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User does not exists!" });

            var userpass = await _userManager.CheckPasswordAsync(user, userObj.PasswordHash);
            if (userpass == false)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Invalid Password!" });


            if (user != null && await _userManager.CheckPasswordAsync(user, userObj.PasswordHash))
            {


                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                  //  new Claim(ClaimTypes.Email, user.Email),
                   
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                foreach (var claim in authClaims)
                {
                    await _userManager.AddClaimAsync(user, claim);

                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    //      expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );


                var id = user.Id;
                var name = user.UserName;



                var roleNames = (from userRole in _context.UserRoles
                                 join role in _roleManager.Roles on userRole.RoleId equals role.Id
                                 where user.Id == userRole.UserId
                                 select role);


                Useremail = user.Email;
                userName = user.UserName;

                int specialityId = 0;

                var lstDoctors = _context.Doctors.Where(a => a.Email == Useremail).ToList();
                if (lstDoctors.Count > 0)
                {
                    specialityId = int.Parse(lstDoctors[0].SpecialistId.ToString());
                }


                return Ok(new
                {
                    id = id,
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    email = Useremail,
                    userName = userName,
                    roles = userRoles,
                    expiration = token.ValidTo,
                    specialityId = specialityId,
                    roleNames = roleNames,
                    Message = "success"
                });
            }
            return Unauthorized();
        }



        private string GetPasswordValidationErrors(IEnumerable<IdentityError> errors)
        {
            var errorMessages = errors.Select(error => $"{error.Code} : {error.Description} \n").ToList();
            return string.Join("\n", errorMessages);
        }



        [HttpDelete]
        [Route("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            // Find the user by ID
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Delete the user
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { message = "User deleted successfully" });
            }
            else
            {
                return BadRequest(new { message = result.Errors });
            }
        }



        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Create(ApplicationUser userObj)
        {
            var userExists = await _userManager.FindByNameAsync(userObj.UserName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "username", Message = "User already exists!", MessageAr = "هذا الاسم مسجل مسبقاً" });

            var userEmailExists = await _userManager.FindByEmailAsync(userObj.Email);
            if (userEmailExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "email", Message = "User email already exists!", MessageAr = "هذا البريد الإلكتروني مسجل مسبقاً" });


            //var validationResult = await _passwordValidatorService.ValidateAsync(_userManager, new ApplicationUser { UserName = userObj.UserName }, userObj.PasswordHash);
            //if (!validationResult.Succeeded)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "password", Message = GetPasswordValidationErrors(validationResult.Errors) });
            //}

            var password = userObj.PasswordHash;

            ApplicationUser user = new ApplicationUser();
            user.UserName = userObj.UserName;
            user.Email = userObj.Email;
            user.PhoneNumber = userObj.PhoneNumber;
            var userResult = await _userManager.CreateAsync(user, password);


            if (!userResult.Succeeded)
            {
                foreach (var error in userResult.Errors)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = error.Description, MessageAr = error.Description });
                }
            }
            else
            {
                if (userObj.RoleNames?.Count > 0)
                {
                    foreach (var role in userObj.RoleNames)
                    {
                        //if (role == "")
                        //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "role", Message ="Please select doctor role", MessageAr = "اختر دور الدكتور" });
                        //else
                        //{
                            string? roleName = _roleManager?.Roles?.Where(a => a.Name == role).FirstOrDefault().Name;
                            await _userManager.AddToRoleAsync(user, roleName);
                       // }
                    }
                }
            }
            return Ok();
        }




        [HttpGet]
        [Route("ListOfRegisteredDoctors")]
        public async Task<List<IndexUserVM.GetData>> ListOfRegisteredDoctors()
        {
            List<IndexUserVM.GetData> lstUsers = new List<IndexUserVM.GetData>();



            var role = await _roleManager.FindByNameAsync("Doctor");
            if (role == null)
            {
                return new List<IndexUserVM.GetData>();
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            foreach (var item in usersInRole)
            {
                IndexUserVM.GetData userObj = new IndexUserVM.GetData();
                userObj.Id = item.Id;
                userObj.UserName = item.UserName;
                lstUsers.Add(userObj);
            }
            return lstUsers;
        }





        [HttpGet]
        [Route("ListOfRegisteredDoctorsBySpecialityId/{specialityId}")]
        public async Task<List<IndexUserVM.GetData>> ListOfRegisteredDoctors(int specialityId)
        {
            List<IndexUserVM.GetData> lstUsers = new List<IndexUserVM.GetData>();
            var lstDoctors = _context.Doctors.Where(a => a.SpecialistId == specialityId && a.IsActive == true).ToList();
            if (lstDoctors.Count > 0)
            {
                var role = await _roleManager.FindByNameAsync("Doctor");
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                var doctorIds = lstDoctors.Select(d => d.Email).ToList(); // Assuming Doctor has a property UserId that links to the user

                foreach (var item in usersInRole)
                {
                    if (doctorIds.Contains(item.Email)) // Check if the user is a doctor in the filtered list
                    {
                        IndexUserVM.GetData userObj = new IndexUserVM.GetData();
                        var doctorObj = _context.Doctors.Where(a => a.Email == item.Email).ToList();
                        userObj.Id = item.Id;
                        userObj.UserName = item.UserName;
                        lstUsers.Add(userObj);
                    }
                }
            }
            return lstUsers;
        }



        [HttpGet]
        [Route("ListOfRegisteredSupervisorDoctors")]
        public async Task<List<IndexUserVM.GetData>> ListOfRegisteredSupervisorDoctors()
        {
            List<IndexUserVM.GetData> lstUsers = new List<IndexUserVM.GetData>();



            var role = await _roleManager.FindByNameAsync("SupervisorDoctor");
            if (role == null)
            {
                return new List<IndexUserVM.GetData>();
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            foreach (var item in usersInRole)
            {
                IndexUserVM.GetData userObj = new IndexUserVM.GetData();
                userObj.Id = item.Id;
                userObj.UserName = item.UserName;
                lstUsers.Add(userObj);
            }
            return lstUsers;
        }









        [HttpGet]
        [Route("ListOfRegisteredSupervisorDoctorsBySpecialityId/{specialityId}")]
        public async Task<List<IndexUserVM.GetData>> ListOfRegisteredSupervisorDoctorsBySpecialityId(int specialityId)
        {
            List<IndexUserVM.GetData> lstUsers = new List<IndexUserVM.GetData>();
            var lstDoctors = _context.Doctors.Where(a => a.SpecialistId == specialityId && a.IsActive == true && a.ParentId == 0).ToList();
            if (lstDoctors.Count > 0)
            {
                var role = await _roleManager.FindByNameAsync("SupervisorDoctor");
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                var doctorIds = lstDoctors.Select(d => d.Email).ToList(); // Assuming Doctor has a property UserId that links to the user

                foreach (var item in usersInRole)
                {
                    if (doctorIds.Contains(item.Email)) // Check if the user is a doctor in the filtered list
                    {
                        IndexUserVM.GetData userObj = new IndexUserVM.GetData();
                        var doctorObj = _context.Doctors.Where(a => a.Email == item.Email).ToList();
                        userObj.Id = item.Id;
                        userObj.UserName = item.UserName;
                        lstUsers.Add(userObj);
                    }
                }
            }
            return lstUsers;
        }


        //[HttpPost("ForgotPassword")]
        //public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordVM forgotPasswordModel)
        //{
        //    string replace = "";
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
        //    if (user == null)
        //        return BadRequest("Invalid Request");

        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    var param = new Dictionary<string, string>
        //     {
        //         {"email", forgotPasswordModel.Email },
        //         {"token", token }

        //     };

        //    var callback = QueryHelpers.AddQueryString(forgotPasswordModel.ClientURI, param);
        //    var hash = callback.Split("#");
        //    var query = hash[0];
        //    replace = query.Replace("/?", "/#/reset?");


        //    // replace = query.Replace("ResetPassword?", "#/ResetPassword?");


        //    StringBuilder strBuild = new StringBuilder();
        //    strBuild.Append("Dear " + user.UserName + ":");
        //    strBuild.Append("<br />");
        //    strBuild.Append("من فضلك اضغط على الرابط التالي لتغيير كلمة المرور");
        //    strBuild.Append("<br />");
        //    strBuild.Append("<a href='" + replace + "'>اضغط هنا</a>");


        //    string from = "almostakbaltechnology.dev@gmail.com";
        //    string subject = "Al-Mostakbal Technology";
        //    string body = strBuild.ToString();
        //    string appSpecificPassword = "fajtjigwpcnxyyuv";


        //    var mailMessage = new MailMessage(from, user.Email, subject, body);
        //    mailMessage.IsBodyHtml = true;
        //    using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
        //    {
        //        smtpClient.EnableSsl = true;
        //        smtpClient.Credentials = new NetworkCredential(from, appSpecificPassword);
        //        smtpClient.Send(mailMessage);
        //    }





        //    return Ok();
        //}
        //[HttpPost("ResetPassword")]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordVM resetPasswordDto)
        //{
        //    List<string> lstErrors = new List<string>();
        //    string errormessage = "";
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        //    if (user == null)
        //        return BadRequest("Invalid Request");

        //    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    var resetPassResult = await _userManager.ResetPasswordAsync(user, code, resetPasswordDto.Password);
        //    if (!resetPassResult.Succeeded)
        //    {
        //        var errors = resetPassResult.Errors.Select(e => e.Description);

        //        foreach (var error in errors)
        //        {
        //            lstErrors.Add(error);

        //        }
        //        errormessage = string.Join("<br />", lstErrors);
        //        return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = errormessage, MessageAr = errormessage });
        //    }

        //    return Ok(lstErrors);
        //}




    }
}
