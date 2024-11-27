using BannerConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.BannerVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BannerConsult.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class BannerController : ControllerBase
    {

        private IBannerRepository _BannerRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        IWebHostEnvironment _webHostingEnvironment;

        public BannerController(IBannerRepository BannerRepository, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostingEnvironment)
        {
            _BannerRepository = BannerRepository;
            _userManager = userManager;
            _webHostingEnvironment = webHostingEnvironment;
        }


        [HttpPost]
        [Route("ListBanners/{pageNumber}/{pageSize}")]
        public async Task<IndexBannerVM> GetAll(SortAndFilterBannerVM data, int pageNumber, int pageSize)
        {
            return await _BannerRepository.GetAll(data, pageNumber, pageSize);
        }

        [HttpGet]
        [Route("ListAllBanners")]
        public List<IndexBannerVM.GetData> GetAll()
        {
            return  _BannerRepository.GetAll();
        }

        //[HttpPost]
        //[Route("CheckBannerRole")]
        //public async Task<List<IndexUserVM.GetData>> CheckBannerRole(BannerUserRole BannerUserRoleObj)
        //{
        //    return await _BannerRepository.CheckBannerRole(BannerUserRoleObj);
        //}




     //[HttpGet]
     //   [Route("GetBannersBySpecialityId/{specialityId}")]
     //   public async Task<List<IndexUserVM.GetData>> GetBannersBySpecialityId(int specialityId)
     //   {
     //       return await _BannerRepository.GetBannersBySpecialityId(specialityId);
     //   }

        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditBannerVM> GetById(int id)
        {
            return _BannerRepository.GetById(id);
        }

        [HttpPut]
        [Route("UpdateBanner")]
        public IActionResult Update(EditBannerVM BannerVM)
        {
            try
            {
                int updatedRow = _BannerRepository.Update(BannerVM);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddBanner")]
        public IActionResult Add(CreateBannerVM BannerVM)
        {
            var savedId = _BannerRepository.Add(BannerVM);
            return Ok(savedId);
        }

        [HttpDelete]
        [Route("DeleteBanner/{id}")]
        public ActionResult<Banner> Delete(int id)
        {
            try
            {
                int deletedRow = _BannerRepository.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }


        [HttpPost]
        [Route("UploadBannerImage")]
        public ActionResult UploadBannerImage(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/BannerImages";
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
        [Route("UpdateBannerImageAfterInsert")]
        public IActionResult UpdateBannerImageAfterInsert(CreateBannerVM modelObj)
        {
            try
            {
                int updatedRow = _BannerRepository.UpdateBannerImageAfterInsert(modelObj);
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
