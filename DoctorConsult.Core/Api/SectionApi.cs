using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.SectionVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace SectionConsult.Core.Repositories
{
    public class SectionApi : ISectionRepository
    {
        private ApplicationDbContext _context;

        public SectionApi(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create Section
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(CreateSectionVM model)
        {

            try
            {
                if (model != null)
                {
                    Section SectionObj = new Section();
                    SectionObj.Title = model.Title;
                    SectionObj.TitleAr = model.TitleAr;
                    SectionObj.Brief = model.Brief;
                    SectionObj.BriefAr = model.BriefAr;
                    SectionObj.SectionDesc = model.SectionDesc;
                    SectionObj.SectionDescAr = model.SectionDescAr;
                    SectionObj.SectionImg = model.SectionImg;
                    SectionObj.IsInAbout = model.IsInAbout;
                    _context.Sections.Add(SectionObj);
                    _context.SaveChanges();
                    return SectionObj.Id;
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
            var SectionObj = _context.Sections.Find(id);
            try
            {
                _context.Sections.Remove(SectionObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        /// <summary>
        /// List Activated Sections
        /// </summary>
        /// <returns></returns>
        public List<IndexSectionVM.GetData> GetActivatedSections()
        {
            List<IndexSectionVM.GetData> list = new List<IndexSectionVM.GetData>();

            var lstSections = _context.Sections.ToList();


            var countItems = lstSections.ToList();

            if (lstSections.Count > 0)
            {
                foreach (var item in lstSections)
                {
                    IndexSectionVM.GetData getDataObj = new IndexSectionVM.GetData();
                    getDataObj.Title = item.Title;
                    getDataObj.TitleAr = item.TitleAr;
                    getDataObj.Brief = item.Brief;
                    getDataObj.BriefAr = item.BriefAr;
                    getDataObj.SectionImg = item.SectionImg;
                    list.Add(getDataObj);
                }
            }

            return list;
        }

        /// <summary>
        /// List Activated Sections by SpecialityId
        /// </summary>
        /// <param name="specialityId"></param>
        /// <returns></returns>
        public List<IndexSectionVM.GetData> GetActivatedSectionsBySpecialityId(int specialityId)
        {
            List<IndexSectionVM.GetData> list = new List<IndexSectionVM.GetData>();

            var lstSections = _context.Sections.ToList();


            var countItems = lstSections.ToList();

            if (lstSections.Count > 0)
            {
                foreach (var item in lstSections)
                {
                    IndexSectionVM.GetData getDataObj = new IndexSectionVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Title = item.Title;
                    getDataObj.TitleAr = item.TitleAr;
                    getDataObj.Brief = item.Brief;
                    getDataObj.BriefAr = item.BriefAr;
                    getDataObj.SectionImg = item.SectionImg;
                    list.Add(getDataObj);
                }
            }

            return list;
        }

        /// <summary>
        /// List All Sections
        /// </summary>
        /// <returns></returns>
        public List<IndexSectionVM.GetData> GetAll()
        {
            List<IndexSectionVM.GetData> list = new List<IndexSectionVM.GetData>();

            var lstSections = _context.Sections.ToList();


            var countItems = lstSections.ToList();

            if (lstSections.Count > 0)
            {
                foreach (var item in lstSections)
                {
                    IndexSectionVM.GetData getDataObj = new IndexSectionVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Title = item.Title;
                    getDataObj.TitleAr = item.TitleAr;
                    getDataObj.Brief = item.Brief;
                    getDataObj.BriefAr = item.BriefAr;
                    getDataObj.SectionDesc = item.SectionDesc;
                    getDataObj.SectionDescAr = item.SectionDescAr;
                    getDataObj.SectionImg = item.SectionImg;
 getDataObj.IsInAbout = item.IsInAbout;

                    list.Add(getDataObj);
                }
            }

            return list;
        }



        /// <summary>
        /// Get Section By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EditSectionVM GetById(int id)
        {
            var SectionObj = _context.Sections.Where(a => a.Id == id).Select(item => new EditSectionVM
            {
                Id = item.Id,
                Title = item.Title,
                TitleAr = item.TitleAr,
                Brief = item.Brief,
                BriefAr = item.BriefAr,
                SectionDesc = item.SectionDesc,
                SectionDescAr = item.SectionDescAr,
                SectionImg = item.SectionImg,
                IsInAbout=item.IsInAbout

            }).First();

            return SectionObj;
        }

        public List<IndexSectionVM.GetData> SelectSectionsInAbout()
        {
            List<IndexSectionVM.GetData> list = new List<IndexSectionVM.GetData>();

            var lstSections = _context.Sections.Where(a=>a.IsInAbout == true).ToList();


            var countItems = lstSections.ToList();

            if (lstSections.Count > 0)
            {
                foreach (var item in lstSections)
                {
                    IndexSectionVM.GetData getDataObj = new IndexSectionVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Title = item.Title;
                    getDataObj.TitleAr = item.TitleAr;
                    getDataObj.Brief = item.Brief;
                    getDataObj.BriefAr = item.BriefAr;
                    getDataObj.SectionDesc = item.SectionDesc;
                    getDataObj.SectionDescAr = item.SectionDescAr;
                    getDataObj.SectionImg = item.SectionImg;
                    getDataObj.IsInAbout = item.IsInAbout;
                    list.Add(getDataObj);
                }
            }

            return list;
        }

        /// <summary>
        /// Update Section
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(EditSectionVM model)
        {
            try
            {
                var SectionObj = _context.Sections.Find(model.Id);
                SectionObj.Id = model.Id;
                SectionObj.Title = model.Title;
                SectionObj.TitleAr = model.TitleAr;
                SectionObj.Brief = model.Brief;
                SectionObj.BriefAr = model.BriefAr;
                SectionObj.SectionDesc = model.SectionDesc;
                SectionObj.SectionDescAr = model.SectionDescAr;
                SectionObj.SectionImg = model.SectionImg;
                SectionObj.IsInAbout = model.IsInAbout;
                _context.Entry(SectionObj).State = EntityState.Modified;
                _context.SaveChanges();
                return SectionObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        /// <summary>
        /// Update SectionImage After Insert
        /// </summary>
        /// <param name="modelObj"></param>
        /// <returns></returns>
        public int UpdateSectionImageAfterInsert(CreateSectionVM modelObj)
        {
            var SectionObj = _context.Sections.Find(modelObj.Id);
            SectionObj.SectionImg = modelObj.SectionImg;
            _context.Entry(SectionObj).State = EntityState.Modified;
            _context.SaveChanges();
            return SectionObj.Id;
        }
    }
}
