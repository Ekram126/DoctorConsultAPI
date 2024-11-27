
using DoctorConsult.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoctorConsult.ViewModels.PatientVM;
using DoctorConsult.Models;
using DoctorConsult.Web.Helpers;

namespace DoctorConsult.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private IPatientRepository _patientRepository;
        UserManager<ApplicationUser> _userManager;
        public PatientController(IPatientRepository patientRepository, UserManager<ApplicationUser> userManager)
        {
            _patientRepository = patientRepository;
            _userManager = userManager;
        }


        [HttpPost]
        [Route("ListPatients/{pageNumber}/{pageSize}")]
        public async Task<IndexPatientVM> GetAll(SortAndFilterPatientVM data, int pageNumber, int pageSize)
        {
            return await _patientRepository.GetAll(data, pageNumber, pageSize);
        }



 
        


        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditPatientVM> GetById(int id)
        {
            return _patientRepository.GetById(id);
        }



        [HttpPut]
        [Route("UpdatePatient")]
        public IActionResult Update(EditPatientVM PatientVM)
        {
            try
            {

                int updatedRow = _patientRepository.Update(PatientVM);


            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddPatient")]
        public IActionResult Add(CreatePatientVM PatientVM)
        {

            var emailExists = _patientRepository.GetAll().Where(a => a.Email == PatientVM.Email).ToList();
            if (emailExists.Count > 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "email", Message = "User email already exists!", MessageAr = "هذا البريد الإلكتروني مسجل مسبقاً" });

            var savedId = _patientRepository.Add(PatientVM);
            return Ok();


        }

        [HttpDelete]
        [Route("DeletePatient/{id}")]
        public ActionResult<Patient> Delete(int id)
        {
            try
            {
                int deletedRow = _patientRepository.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }

        [HttpGet]
        [Route("GeneratePatientNumber")]
        public GeneratedPatientNumberVM GeneratePatientNumber()
        {
            return _patientRepository.GeneratePatientNumber();
        }

    }
}
