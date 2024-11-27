using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.SectionVM;
using DoctorConsult.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorConsult.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class SectionController : ControllerBase
    {

        private ISectionRepository _SectionRepository;
        IWebHostEnvironment _webHostingEnvironment;

        public SectionController(ISectionRepository SectionRepository, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostingEnvironment)
        {
            _SectionRepository = SectionRepository;
            _webHostingEnvironment = webHostingEnvironment;
        }





        



        [HttpGet]
        [Route("ListSections")]
        public List<IndexSectionVM.GetData> GetAll()
        {
            return _SectionRepository.GetAll();
        }

       
      

        [HttpGet]
        [Route("SelectSectionsInAbout")]
        public List<IndexSectionVM.GetData> SelectSectionsInAbout()
        {
            return _SectionRepository.SelectSectionsInAbout();
        }

       

        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditSectionVM> GetById(int id)
        {
            return _SectionRepository.GetById(id);
        }

        [HttpPut]
        [Route("UpdateSection")]
        public IActionResult Update(EditSectionVM SectionVM)
        {
            try
            {
                int updatedRow = _SectionRepository.Update(SectionVM);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddSection")]
        public IActionResult Add(CreateSectionVM SectionVM)
        {
            var savedId = _SectionRepository.Add(SectionVM);
            return Ok(savedId);
        }

        [HttpDelete]
        [Route("DeleteSection/{id}")]
        public ActionResult<Section> Delete(int id)
        {
            try
            {
                int deletedRow = _SectionRepository.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }

        [HttpPut]
        [Route("UpdateSectionImageAfterInsert")]
        public IActionResult UpdateSectionImageAfterInsert(CreateSectionVM modelObj)
        {
            try
            {
           
                    int updatedRow = _SectionRepository.UpdateSectionImageAfterInsert(modelObj);
                    return Ok(modelObj.Id);
          
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }            
        }


        [HttpPost]
        [Route("UploadSectionImage")]
        public ActionResult UploadSectionImage(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SectionImages";
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
