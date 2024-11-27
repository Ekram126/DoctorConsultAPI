using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.VideoVM;
using DoctorConsult.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorConsult.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class VideoController : ControllerBase
    {

        private IVideoRepository _videoRepository;
        IWebHostEnvironment _webHostingEnvironment;

        public VideoController(IVideoRepository videoRepository, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostingEnvironment)
        {
            _videoRepository = videoRepository;
            _webHostingEnvironment = webHostingEnvironment;
        }


        [HttpPost]
        [Route("ListVideos/{pageNumber}/{pageSize}")]
        public async Task<IndexVideoVM> GetAll(SortAndFilterVideoVM data, int pageNumber, int pageSize)
        {
            return await _videoRepository.GetAll(data, pageNumber, pageSize);
        }



        [HttpGet]
        [Route("ListAllVideos")]
        public List<IndexVideoVM.GetData> GetAll()
        {
            return _videoRepository.GetAll();
        }

        [HttpGet]
        [Route("GetActivatedVideos")]
        public List<IndexVideoVM.GetData> GetActivatedVideos()
        {
            return _videoRepository.GetActivatedVideos();
        }



        [HttpGet]
        [Route("GetActivatedVideosBySpecialityId/{specialityId}")]
        public List<IndexVideoVM.GetData> GetActivatedVideosBySpecialityId(int specialityId)
        {
            return _videoRepository.GetActivatedVideosBySpecialityId(specialityId);
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditVideoVM> GetById(int id)
        {
            return _videoRepository.GetById(id);
        }

        [HttpPut]
        [Route("UpdateVideo")]
        public IActionResult Update(EditVideoVM VideoVM)
        {
            try
            {
                int updatedRow = _videoRepository.Update(VideoVM);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddVideo")]
        public IActionResult Add(CreateVideoVM VideoVM)
        {
            var savedId = _videoRepository.Add(VideoVM);
            if (savedId == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "video", Message = "This Youtube Url is Invalid!", MessageAr = "هذا الرابط غير صحيح" });
            }

            return Ok(savedId);
        }

        [HttpDelete]
        [Route("DeleteVideo/{id}")]
        public ActionResult<Video> Delete(int id)
        {
            try
            {
                int deletedRow = _videoRepository.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }




     
    }
}
