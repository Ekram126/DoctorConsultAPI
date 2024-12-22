using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.ArticleVM;
using DoctorConsult.ViewModels.DoctorVM;
using DoctorConsult.ViewModels.RequestVM;
using DoctorConsult.ViewModels.UserVM;
using DoctorConsult.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DoctorConsult.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class DoctorController : ControllerBase
    {

        private IDoctorRepository _doctorRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        IWebHostEnvironment _webHostingEnvironment;

        public DoctorController(IDoctorRepository doctorRepository, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostingEnvironment)
        {
            _doctorRepository = doctorRepository;
            _userManager = userManager;
            _webHostingEnvironment = webHostingEnvironment;
        }


        [HttpPost]
        [Route("ListDoctors/{pageNumber}/{pageSize}")]
        public async Task<IndexDoctorVM> GetAll(SortAndFilterDoctorVM data, int pageNumber, int pageSize)
        {
            return await _doctorRepository.GetAll(data, pageNumber, pageSize);
        }

        [HttpGet]
        [Route("ListAllDoctors")]
        public List<IndexDoctorVM.GetData> GetAll()
        {
            return _doctorRepository.GetAll();
        }

        [HttpPost]
        [Route("CheckDoctorRole")]
        public async Task<List<IndexUserVM.GetData>> CheckDoctorRole(DoctorUserRole doctorUserRoleObj)
        {
            return await _doctorRepository.CheckDoctorRole(doctorUserRoleObj);
        }

        [HttpGet]
        [Route("GenerateDoctorCode")]
        public GeneratedDoctorCodeVM GenerateDoctorCode()
        {
            return _doctorRepository.GenerateDoctorCode();
        }


        [HttpGet]
        [Route("GetDoctorsBySpecialityId/{specialityId}")]
        public async Task<List<IndexUserVM.GetData>> GetDoctorsBySpecialityId(int specialityId)
        {
            return await _doctorRepository.GetDoctorsBySpecialityId(specialityId);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditDoctorVM> GetById(int id)
        {
            return _doctorRepository.GetById(id);
        }

        [HttpPut]
        [Route("UpdateDoctor")]
        public IActionResult Update(EditDoctorVM DoctorVM)
        {
            try
            {
                int id = DoctorVM.Id;
                var codeExist = _doctorRepository.GetAll().ToList().Where(a => a.Code == DoctorVM.Code && a.Id != id).ToList();
                if (codeExist.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "This code already exist!", MessageAr = "هذا الكود مسجل مسبقاً" });
                }
                else
                {

                    int updatedRow = _doctorRepository.Update(DoctorVM);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddDoctor")]
        public IActionResult Add(CreateDoctorVM DoctorVM)
        {

            var emailExists = _doctorRepository.GetAll().Where(a => a.Email == DoctorVM.Email).ToList();
            if (emailExists.Count > 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "email", Message = "User email already exists!", MessageAr = "هذا البريد الإلكتروني مسجل مسبقاً" });

            var codeExists = _doctorRepository.GetAll().Where(a => a.Code == DoctorVM.Code).ToList();
            if (codeExists.Count > 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "This code already exist!", MessageAr = "هذا الكود مسجل مسبقاً" });


            var savedId = _doctorRepository.Add(DoctorVM);
            return Ok(savedId);
        }

        [HttpDelete]
        [Route("DeleteDoctor/{id}")]
        public ActionResult<Doctor> Delete(int id)
        {
            try
            {
                int deletedRow = _doctorRepository.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }


        [HttpPost]
        [Route("UploadDoctorImage")]
        public ActionResult UploadDoctorImage(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/DoctorImages";
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



        [HttpPut]
        [Route("UpdateDoctorImageAfterInsert")]
        public IActionResult UpdateDoctorImageAfterInsert(CreateDoctorVM modelObj)
        {
            try
            {
                int updatedRow = _doctorRepository.UpdateDoctorImageAfterInsert(modelObj);
                return Ok(modelObj.Id);

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }
        }


    }
}
