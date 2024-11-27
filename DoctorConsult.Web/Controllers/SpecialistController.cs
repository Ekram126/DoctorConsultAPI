
using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.SpecialistVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SpecialistConsult.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SpecialistController : ControllerBase
    {

        private ISpecialistRepository _specialistRepository;
        IWebHostEnvironment _webHostingEnvironment;

        public SpecialistController(ISpecialistRepository specialistRepository, IWebHostEnvironment webHostingEnvironment)
        {
            _specialistRepository = specialistRepository;
            _webHostingEnvironment = webHostingEnvironment;

        }


        [HttpPost]
        [Route("ListSpecialistsByPages/{pageNumber}/{pageSize}")]
        public IndexSpecialistVM GetAll(SortAndFilterSpecialistVM data, int pageNumber, int pageSize)
        {
            return _specialistRepository.GetAll(data, pageNumber, pageSize);
        }



        [HttpGet]
        [Route("ListSpecialists")]
        public List<Specialist> GetAll()
        {
            return _specialistRepository.GetAll();
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditSpecialistVM> GetById(int id)
        {
            return _specialistRepository.GetById(id);
        }


        [HttpGet]
        [Route("GenerateSpecialityNumber")]
        public ActionResult<GenerateSpecialityNumberVM> GenerateSpecialityNumber()
        {
            return _specialistRepository.GenerateSpecialityNumber();
        }


        [HttpGet]
        [Route("AutoCompleteSpecialityName/{name}")]
        public IEnumerable<Specialist> AutoCompleteSpecialityName(string name)
        {
            return _specialistRepository.AutoCompleteSpecialityName(name);
        }


        [HttpPut]
        [Route("UpdateSpecialist")]
        public IActionResult Update(EditSpecialistVM SpecialistVM)
        {
            try
            {

                int updatedRow = _specialistRepository.Update(SpecialistVM);


            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddSpecialist")]
        public int Add(CreateSpecialistVM SpecialistVM)
        {
            var savedId = _specialistRepository.Add(SpecialistVM);
            return savedId;


        }

        [HttpDelete]
        [Route("DeleteSpecialist/{id}")]
        public ActionResult<Specialist> Delete(int id)
        {
            try
            {
                int deletedRow = _specialistRepository.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }


        [HttpPost]
        [Route("UploadSpecialityFile")]
        public ActionResult UploadRequestFiles(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SprcialityFiles";
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

    }
}
