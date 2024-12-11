using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.ArticleVM;
using DoctorConsult.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorConsult.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class ArticleController : ControllerBase
    {

        private IArticleRepository _articleRepository;
        IWebHostEnvironment _webHostingEnvironment;

        public ArticleController(IArticleRepository articleRepository, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostingEnvironment)
        {
            _articleRepository = articleRepository;
            _webHostingEnvironment = webHostingEnvironment;
        }





        /// <summary>
        /// Gets all articles.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ListArticles/{pageNumber}/{pageSize}")]
        public async Task<IndexArticleVM> GetAll(SortAndFilterArticleVM data, int pageNumber, int pageSize)
        {
            return await _articleRepository.GetAll(data, pageNumber, pageSize);
        }



        [HttpGet]
        [Route("ListAllArticles")]
        public List<IndexArticleVM.GetData> GetAll()
        {
            return _articleRepository.GetAll();
        }

        [HttpGet]
        [Route("GetActivatedArticles")]
        public List<IndexArticleVM.GetData> GetActivatedArticles()
        {
            return _articleRepository.GetActivatedArticles();
        }



        [HttpGet]
        [Route("GetActivatedArticlesBySpecialityId/{specialityId}")]
        public List<IndexArticleVM.GetData> GetActivatedArticlesBySpecialityId(int specialityId)
       {
            return _articleRepository.GetActivatedArticlesBySpecialityId(specialityId);
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditArticleVM> GetById(int id)
        {
            return _articleRepository.GetById(id);
        }

        [HttpPut]
        [Route("UpdateArticle")]
        public IActionResult Update(EditArticleVM ArticleVM)
        {
            try
            {

                int id = ArticleVM.Id;
                var lstarticles = _articleRepository.GetAll().ToList().Where(a => a.OrderId == ArticleVM.OrderId && a.Id != id).ToList();
                if (lstarticles.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "art", Message = "This order already exist!", MessageAr = "هذا الترتيب مسجل مسبقاً" });
                }
                else
                {

                    int updatedRow = _articleRepository.Update(ArticleVM);
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
        [Route("AddArticle")]
        public IActionResult Add(CreateArticleVM ArticleVM)
        {
            var existCode = _articleRepository.GetAll().Where(a => a.OrderId == ArticleVM.OrderId).ToList();
            if (existCode.Count > 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "special", Message = "This order already exist!", MessageAr = "هذا الترتيب مسجل مسبقاً" });


            var savedId = _articleRepository.Add(ArticleVM);
            return Ok(savedId);
        }

        [HttpDelete]
        [Route("DeleteArticle/{id}")]
        public ActionResult<Article> Delete(int id)
        {
            try
            {
                int deletedRow = _articleRepository.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }

        [HttpPut]
        [Route("UpdateArticleImageAfterInsert")]
        public IActionResult UpdateArticleImageAfterInsert(CreateArticleVM modelObj)
        {
            try
            {

                    int updatedRow = _articleRepository.UpdateArticleImageAfterInsert(modelObj);
                    return Ok(modelObj.Id);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }            
        }


        [HttpPost]
        [Route("UploadArticleImage")]
        public ActionResult UploadArticleImage(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/ArticleImages";
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







        [HttpPost]
        [Route("UploadArticleEditorImages")]
        public ActionResult UploadArticleEditorImages(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/ArticleEditorImages";
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
