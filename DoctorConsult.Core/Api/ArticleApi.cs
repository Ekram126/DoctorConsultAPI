using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.ArticleVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace ArticleConsult.Core.Repositories
{
    public class ArticleApi : IArticleRepository
    {
        private ApplicationDbContext _context;

        public ArticleApi(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create Article
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(CreateArticleVM model)
        {

            try
            {
                if (model != null)
                {
                    Article articleObj = new Article();
                    articleObj.Title = model.Title;
                    articleObj.TitleAr = model.TitleAr;
                    articleObj.ArticleContent = model.ArticleContent;
                    articleObj.ArticleContentAr = model.ArticleContentAr;
                    articleObj.ArticleDesc = model.ArticleDesc;
                    articleObj.ArticleDescAr = model.ArticleDescAr;
                    articleObj.Date = model.Date;
                    articleObj.ArticleImg = model.ArticleImg;
                    articleObj.SpecialityId = model.specialityId;
                    articleObj.OrderId = model.OrderId;
                    articleObj.IsActive = model.IsActive;
                    _context.Articles.Add(articleObj);
                    _context.SaveChanges();
                    return articleObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;

        }


        /// <summary>
        /// Delete Arcticle By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id)
        {
            var articleObj = _context.Articles.Find(id);
            try
            {
                _context.Articles.Remove(articleObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        /// <summary>
        /// List Activated Articles
        /// </summary>
        /// <returns></returns>
        public List<IndexArticleVM.GetData> GetActivatedArticles()
        {
            List<IndexArticleVM.GetData> list = new List<IndexArticleVM.GetData>();

            var lstArticles = _context.Articles.Include(a => a.Specialist).Where(a=>a.IsActive == true).ToList();


            var countItems = lstArticles.ToList();

            if (lstArticles.Count > 0)
            {
                foreach (var item in lstArticles)
                {
                    IndexArticleVM.GetData getDataObj = new IndexArticleVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Title = item.Title;
                    getDataObj.TitleAr = item.TitleAr;
                    getDataObj.Date = item.Date;
                    getDataObj.ArticleImg = item.ArticleImg;
                    getDataObj.SpecialistId = item.SpecialityId;
                    getDataObj.OrderId = item.OrderId;
                    getDataObj.SpecialityName = item.Specialist?.Name;
                    getDataObj.SpecialityNameAr = item.Specialist?.NameAr;
                    getDataObj.IsActive = item.IsActive;
                    list.Add(getDataObj);
                }
            }

            return list;
        }

        /// <summary>
        /// List Activated Articles by SpecialityId
        /// </summary>
        /// <param name="specialityId"></param>
        /// <returns></returns>
        public List<IndexArticleVM.GetData> GetActivatedArticlesBySpecialityId(int specialityId)
        {
            List<IndexArticleVM.GetData> list = new List<IndexArticleVM.GetData>();

            var lstArticles = _context.Articles.Include(a => a.Specialist).Where(a => a.IsActive == true && a.SpecialityId == specialityId).ToList();


            var countItems = lstArticles.ToList();

            if (lstArticles.Count > 0)
            {
                foreach (var item in lstArticles)
                {
                    IndexArticleVM.GetData getDataObj = new IndexArticleVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Title = item.Title;
                    getDataObj.TitleAr = item.TitleAr;
                    getDataObj.Date = item.Date;
                    getDataObj.ArticleImg = item.ArticleImg;
                    getDataObj.SpecialistId = item.SpecialityId;
                    getDataObj.OrderId = item.OrderId;
                    getDataObj.SpecialityName = item.Specialist?.Name;
                    getDataObj.SpecialityNameAr = item.Specialist?.NameAr;
                    getDataObj.IsActive = item.IsActive;
                    list.Add(getDataObj);
                }
            }

            return list;
        }

        /// <summary>
        /// List All Articles
        /// </summary>
        /// <returns></returns>
        public List<IndexArticleVM.GetData> GetAll()
        {
            List<IndexArticleVM.GetData> list = new List<IndexArticleVM.GetData>();

            var lstArticles = _context.Articles.Include(a => a.Specialist).ToList();


            var countItems = lstArticles.ToList();

            if (lstArticles.Count > 0)
            {
                foreach (var item in lstArticles)
                {
                    IndexArticleVM.GetData getDataObj = new IndexArticleVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Title = item.Title;
                    getDataObj.TitleAr = item.TitleAr;
                    getDataObj.Date = item.Date;
                    getDataObj.ArticleImg = item.ArticleImg;
                    getDataObj.SpecialistId = item.SpecialityId;
                    getDataObj.OrderId = item.OrderId;
                    getDataObj.SpecialityName = item.Specialist?.Name;
                    getDataObj.SpecialityNameAr = item.Specialist?.NameAr;
                    getDataObj.IsActive = item.IsActive;
                    list.Add(getDataObj);
                }
            }

            return list;
        }


        /// <summary>
        /// List Articles with sort and search parameters and pagenumber and page size
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IndexArticleVM> GetAll(SortAndFilterArticleVM data, int pageNumber, int pageSize)
        {

            IndexArticleVM mainClass = new IndexArticleVM();
            List<IndexArticleVM.GetData> list = new List<IndexArticleVM.GetData>();
            var lstArticles = _context.Articles.Include(a => a.Specialist).ToList();



            #region Sort Criteria

            switch (data.SortObj?.SortBy)
            {
                case "Title":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstArticles = lstArticles.OrderBy(x => x.Title).ToList();
                    }
                    else
                    {
                        lstArticles = lstArticles.OrderByDescending(x => x.Title).ToList();
                    }
                    break;
                case "العنوان":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstArticles = lstArticles.OrderBy(x => x.TitleAr).ToList();
                    }
                    else
                    {
                        lstArticles = lstArticles.OrderByDescending(x => x.TitleAr).ToList();
                    }
                    break;


                case "Speciality":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstArticles = lstArticles.OrderBy(x => x.Specialist.Name).ToList();
                    }
                    else
                    {
                        lstArticles = lstArticles.OrderByDescending(x => x.Specialist.Name).ToList();
                    }
                    break;
                case "التخصص":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstArticles = lstArticles.OrderBy(x => x.Specialist.NameAr).ToList();
                    }
                    else
                    {
                        lstArticles = lstArticles.OrderByDescending(x => x.Specialist.NameAr).ToList();
                    }
                    break;


                case "Date":
                case "التاريخ":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstArticles = lstArticles.OrderBy(x => x.Date.Value.Date).ToList();
                    }
                    else
                    {
                        lstArticles = lstArticles.OrderByDescending(x => x.Date.Value.Date).ToList();
                    }
                    break;
                case "IsActive":
                case "هل هو مفعل":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstArticles = lstArticles.OrderBy(x => x.IsActive).ToList();
                    }
                    else
                    {
                        lstArticles = lstArticles.OrderByDescending(x => x.IsActive).ToList();
                    }
                    break;


            }

            #endregion


            #region Search Criteria
            if (data.SearchObj?.Title != "")
            {
   
                lstArticles = lstArticles.Where(x => x.Title != null && data.SearchObj?.Title != null &&    x.Title.Contains(data.SearchObj.Title)).ToList();
            }
            if (data.SearchObj?.TitleAr != "")
            {
                lstArticles = lstArticles.Where(x => x.TitleAr != null && data.SearchObj?.TitleAr != null && x.TitleAr.Contains(data.SearchObj.TitleAr)).ToList();
            }
            if (data.SearchObj?.SpecialityId != 0)
            {
                lstArticles = lstArticles.Where(x => x.SpecialityId == data.SearchObj?.SpecialityId).ToList();
            }

            //if (data.SearchObj?.IsActive != null)
            //{
            //    lstArticles = lstArticles.Where(x => x.IsActive == data.SearchObj?.IsActive).ToList();
            //}

            string setstartday, setstartmonth, setendday, setendmonth = "";
            DateTime startingFrom = new DateTime();
            DateTime endingTo = new DateTime();
            if (data.SearchObj.Start == "")
            {
                startingFrom = DateTime.Parse("1900-01-01").Date;
            }
            else
            {
                data.SearchObj.StartDate = DateTime.Parse(data.SearchObj.Start.ToString());
                var startyear = data.SearchObj.StartDate.Value.Year;
                var startmonth = data.SearchObj.StartDate.Value.Month;
                var startday = data.SearchObj.StartDate.Value.Day;
                if (startday < 10)
                    setstartday = data.SearchObj.StartDate.Value.Day.ToString().PadLeft(2, '0');
                else
                    setstartday = data.SearchObj.StartDate.Value.Day.ToString();

                if (startmonth < 10)
                    setstartmonth = data.SearchObj.StartDate.Value.Month.ToString().PadLeft(2, '0');
                else
                    setstartmonth = data.SearchObj.StartDate.Value.Month.ToString();

                var sDate = startyear + "/" + setstartmonth + "/" + setstartday;
                startingFrom = DateTime.Parse(sDate);
            }

            if (data.SearchObj.End == "")
            {
                endingTo = DateTime.Today.Date;
            }
            else
            {
                data.SearchObj.EndDate = DateTime.Parse(data.SearchObj.End.ToString());
                var endyear = data.SearchObj.EndDate.Value.Year;
                var endmonth = data.SearchObj.EndDate.Value.Month;
                var endday = data.SearchObj.EndDate.Value.Day;
                if (endday < 10)
                    setendday = data.SearchObj.EndDate.Value.Day.ToString().PadLeft(2, '0');
                else
                    setendday = data.SearchObj.EndDate.Value.Day.ToString();
                if (endmonth < 10)
                    setendmonth = data.SearchObj.EndDate.Value.Month.ToString().PadLeft(2, '0');
                else
                    setendmonth = data.SearchObj.EndDate.Value.Month.ToString();
                var eDate = endyear + "/" + setendmonth + "/" + setendday;
                endingTo = DateTime.Parse(eDate);
            }
            if (data.SearchObj.Start != "" && data.SearchObj.End != "")
            {
                lstArticles = lstArticles.Where(a => a.Date.Value.Date >= startingFrom.Date && a.Date.Value.Date <= endingTo.Date).ToList();
            }

            #endregion
            var countItems = lstArticles.ToList();
            mainClass.Count = countItems.Count();
            if (pageNumber == 0 && pageSize == 0)
                lstArticles = lstArticles.ToList();
            else
                lstArticles = lstArticles.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            if (lstArticles.Count > 0)
            {
                foreach (var item in lstArticles)
                {
                    IndexArticleVM.GetData getDataObj = new IndexArticleVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Title = item.Title;
                    getDataObj.TitleAr = item.TitleAr;
                    getDataObj.Date = item.Date;
                    getDataObj.ArticleImg = item.ArticleImg;
                    getDataObj.SpecialistId = item.SpecialityId;
                    getDataObj.OrderId = item.OrderId;
                    getDataObj.SpecialityName = item.Specialist?.Name;
                    getDataObj.SpecialityNameAr = item.Specialist?.NameAr;
                    getDataObj.IsActive = item.IsActive;
                    list.Add(getDataObj);
                }
            }
            mainClass.Results = list;
            return mainClass;
        }

        /// <summary>
        /// Get Article By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EditArticleVM GetById(int id)
        {
            var articleObj = _context.Articles.Include(a => a.Specialist).Where(a => a.Id == id).Select(item => new EditArticleVM
            {
                Id = item.Id,
                Title = item.Title,
                ArticleContent = item.ArticleContent,
                ArticleContentAr = item.ArticleContentAr,
                TitleAr = item.TitleAr,
                ArticleDesc = item.ArticleDesc,
                ArticleDescAr = item.ArticleDescAr,
                Date = item.Date,
                ArticleImg = item.ArticleImg,
                SpecialityId = item.SpecialityId,
                SpecialityName = item.Specialist != null ? item.Specialist.Name : "",
                SpecialityNameAr = item.Specialist != null ? item.Specialist.NameAr : "",
                OrderId = item.OrderId,
                IsActive = item.IsActive
            }).First();

            return articleObj;
        }

        /// <summary>
        /// Update Article
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(EditArticleVM model)
        {
            try
            {
                var articleObj = _context.Articles.Find(model.Id);
                articleObj.Id = model.Id;
                articleObj.Title = model.Title;
                articleObj.TitleAr = model.TitleAr;
                articleObj.ArticleContent = model.ArticleContent;
                articleObj.ArticleContentAr = model.ArticleContentAr;
                articleObj.ArticleDesc = model.ArticleDesc;
                articleObj.ArticleDescAr = model.ArticleDescAr;
                articleObj.Date = model.Date;
                articleObj.ArticleImg = model.ArticleImg;
                articleObj.SpecialityId = model.SpecialityId;
                articleObj.OrderId = model.OrderId;
                articleObj.IsActive = model.IsActive;
                _context.Entry(articleObj).State = EntityState.Modified;
                _context.SaveChanges();
                return articleObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        /// <summary>
        /// Update ArticleImage After Insert
        /// </summary>
        /// <param name="modelObj"></param>
        /// <returns></returns>
        public int UpdateArticleImageAfterInsert(CreateArticleVM modelObj)
        {
            var articleObj = _context.Articles.Find(modelObj.Id);
            articleObj.ArticleImg = modelObj.ArticleImg;
            _context.Entry(articleObj).State = EntityState.Modified;
            _context.SaveChanges();
            return articleObj.Id;
        }
    }
}
