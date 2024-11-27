using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.ArticleVM;
using DoctorConsult.ViewModels.VideoVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;

namespace videoConsult.Core.Repositories
{
    public class VideoApi : IVideoRepository
    {
        private ApplicationDbContext _context;

        public VideoApi(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(CreateVideoVM model)
        {

            try
            {
                if (model != null)
                {
                    // Validate the VideoURL format
                    if ((!string.IsNullOrEmpty(model.VideoURL)) && (!model.VideoURL.Contains("embed") || !model.VideoURL.Contains("shorts") 
                        || !model.VideoURL.Contains("watch") || !model.VideoURL.StartsWith("https://youtu.be")))
                    {
                        return 0; // You can customize this behavior with a more specific error code or message
                    }
                    else
                    {
                        Video videoObj = new Video();
                        videoObj.VideoURL = model.VideoURL;
                        videoObj.Title = model.Title;
                        videoObj.TitleAr = model.TitleAr;
                        videoObj.Brief = model.Brief;
                        videoObj.BriefAr = model.BriefAr;
                        videoObj.Date = model.Date;
                        videoObj.SpecialityId = model.SpecialityId;
                        videoObj.OrderId = model.OrderId;
                        videoObj.IsActive = model.IsActive;
                        _context.Videos.Add(videoObj);
                        _context.SaveChanges();
                        return videoObj.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;

        }


        public int Delete(int id)
        {
            var videoObj = _context.Videos.Find(id);
            try
            {
                _context.Videos.Remove(videoObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public List<IndexVideoVM.GetData> GetActivatedVideosBySpecialityId(int specialityId)
        {
            List<IndexVideoVM.GetData> list = new List<IndexVideoVM.GetData>();

            var lstvideos = _context.Videos.Include(a => a.Specialist).Where(a => a.IsActive == true && a.SpecialityId == specialityId).ToList();


            var countItems = lstvideos.ToList();

            if (lstvideos.Count > 0)
            {
                foreach (var item in lstvideos)
                {
                    IndexVideoVM.GetData getDataObj = new IndexVideoVM.GetData();
                    getDataObj.Id = item.Id;
                    if (item.VideoURL != null)
                        getDataObj.VideoURL = GetYouTubeVideoId(item.VideoURL);
                    getDataObj.Title = item.Title;
                    getDataObj.TitleAr = item.TitleAr;
                    getDataObj.Date = item.Date;
                    getDataObj.SpecialistId = item.SpecialityId;
                    getDataObj.OrderId = item.OrderId;
                    getDataObj.IsActive = item.IsActive;
                    getDataObj.SpecialityName = item.Specialist?.Name;
                    getDataObj.SpecialityNameAr = item.Specialist?.NameAr;
                    list.Add(getDataObj);
                }
            }

            return list;
        }

        public List<IndexVideoVM.GetData> GetActivatedVideos()
        {
            List<IndexVideoVM.GetData> list = new List<IndexVideoVM.GetData>();

            var lstvideos = _context.Videos.Include(a => a.Specialist).Where(a => a.IsActive == true).ToList();


            var countItems = lstvideos.ToList();

            if (lstvideos.Count > 0)
            {
                foreach (var item in lstvideos)
                {
                    IndexVideoVM.GetData getDataObj = new IndexVideoVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.VideoURL = item.VideoURL;
                    getDataObj.Title = item.Title;
                    getDataObj.TitleAr = item.TitleAr;
                    getDataObj.Date = item.Date;
                    getDataObj.SpecialistId = item.SpecialityId;
                    getDataObj.OrderId = item.OrderId;
                    getDataObj.IsActive = item.IsActive;
                    getDataObj.SpecialityName = item.Specialist?.Name;
                    getDataObj.SpecialityNameAr = item.Specialist?.NameAr;
                    list.Add(getDataObj);
                }
            }

            return list;
        }

        public List<IndexVideoVM.GetData> GetAll()
        {
            List<IndexVideoVM.GetData> list = new List<IndexVideoVM.GetData>();

            var lstvideos = _context.Videos.Include(a => a.Specialist).ToList();


            var countItems = lstvideos.ToList();

            if (lstvideos.Count > 0)
            {
                foreach (var item in lstvideos)
                {
                    IndexVideoVM.GetData getDataObj = new IndexVideoVM.GetData();
                    getDataObj.Id = item.Id;
                    if (item.VideoURL != null)
                        getDataObj.VideoURL = GetYouTubeVideoId(item.VideoURL);
                    getDataObj.Title = item.Title;
                    getDataObj.TitleAr = item.TitleAr;
                    getDataObj.Date = item.Date;
                    getDataObj.SpecialistId = item.SpecialityId;
                    getDataObj.OrderId = item.OrderId;
                    getDataObj.IsActive = item.IsActive;
                    getDataObj.SpecialityName = item.Specialist?.Name;
                    getDataObj.SpecialityNameAr = item.Specialist?.NameAr;
                    list.Add(getDataObj);
                }
            }

            return list;
        }
        public async Task<IndexVideoVM> GetAll(SortAndFilterVideoVM data, int pageNumber, int pageSize)
        {

            IndexVideoVM mainClass = new IndexVideoVM();
            List<IndexVideoVM.GetData> list = new List<IndexVideoVM.GetData>();
            var lstvideos = _context.Videos.Include(a => a.Specialist).ToList();


            #region Sort Criteria

            switch (data.SortObj?.SortBy)
            {
                case "Title":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstvideos = lstvideos.OrderBy(x => x.Title).ToList();
                    }
                    else
                    {
                        lstvideos = lstvideos.OrderByDescending(x => x.Title).ToList();
                    }
                    break;
                case "العنوان":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstvideos = lstvideos.OrderBy(x => x.TitleAr).ToList();
                    }
                    else
                    {
                        lstvideos = lstvideos.OrderByDescending(x => x.TitleAr).ToList();
                    }
                    break;


                case "Speciality":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstvideos = lstvideos.OrderBy(x => x.Specialist.Name).ToList();
                    }
                    else
                    {
                        lstvideos = lstvideos.OrderByDescending(x => x.Specialist.Name).ToList();
                    }
                    break;
                case "التخصص":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstvideos = lstvideos.OrderBy(x => x.Specialist.NameAr).ToList();
                    }
                    else
                    {
                        lstvideos = lstvideos.OrderByDescending(x => x.Specialist.NameAr).ToList();
                    }
                    break;


                case "Date":
                case "التاريخ":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstvideos = lstvideos.OrderBy(x => x.Date.Value.Date).ToList();
                    }
                    else
                    {
                        lstvideos = lstvideos.OrderByDescending(x => x.Date.Value.Date).ToList();
                    }
                    break;
                case "IsActive":
                case "هل هو مفعل":
                    if (data.SortObj?.SortStatus == "ascending")
                    {
                        lstvideos = lstvideos.OrderBy(x => x.IsActive).ToList();
                    }
                    else
                    {
                        lstvideos = lstvideos.OrderByDescending(x => x.IsActive).ToList();
                    }
                    break;


            }

            #endregion

            var countItems = lstvideos.ToList();
            mainClass.Count = countItems.Count();
            if (pageNumber == 0 && pageSize == 0)
                lstvideos = lstvideos.ToList();
            else
                lstvideos = lstvideos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            if (lstvideos.Count > 0)
            {
                foreach (var item in lstvideos)
                {
                    IndexVideoVM.GetData getDataObj = new IndexVideoVM.GetData();
                    getDataObj.Id = item.Id;
                    if (item.VideoURL != null)
                        getDataObj.VideoURL = GetYouTubeVideoId(item.VideoURL);

                    getDataObj.Title = item.Title;
                    getDataObj.TitleAr = item.TitleAr;
                    getDataObj.Date = item.Date;
                    getDataObj.SpecialistId = item.SpecialityId;
                    getDataObj.OrderId = item.OrderId;
                    getDataObj.IsActive = item.IsActive;
                    getDataObj.SpecialityName = item.Specialist?.Name;
                    getDataObj.SpecialityNameAr = item.Specialist?.NameAr;
                    list.Add(getDataObj);
                }
            }
            mainClass.Results = list;
            return mainClass;
        }




        public static string GetYouTubeVideoId(string url)
        {
            // Regex pattern for extracting YouTube video ID from different URL formats
            //    string pattern = @"(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([^""&?\/\s]+)";


            string pattern = @"(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=|shorts\/)|youtu\.be\/)([^""&?\/\s?]+)";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(url);

            // If a match is found, return the video ID
            return match.Success ? match.Groups[1].Value : null;
        }

        public EditVideoVM GetById(int id)
        {
            return _context.Videos.Include(a => a.Specialist).Where(a => a.Id == id).Select(item => new EditVideoVM
            {
                Id = item.Id,
                VideoURL = item.VideoURL != null ? GetYouTubeVideoId(item.VideoURL) : "",
                Title = item.Title,
                TitleAr = item.TitleAr,
                Brief = item.Brief,
                BriefAr = item.BriefAr,
                Date = item.Date,
                SpecialityId = item.SpecialityId,
                OrderId = item.OrderId,
                IsActive = item.IsActive,
                SpecialityName = item.Specialist.Name ?? "",
                SpecialityNameAr = item.Specialist.NameAr ?? ""
            }).First();
        }

        public int Update(EditVideoVM model)
        {
            try
            {
                var videoObj = _context.Videos.Find(model.Id);
                videoObj.Id = model.Id;
                videoObj.VideoURL = model.VideoURL;


                videoObj.Title = model.Title;
                videoObj.TitleAr = model.TitleAr;
                videoObj.Brief = model.Brief;
                videoObj.BriefAr = model.BriefAr;

                videoObj.Date = model.Date;
                videoObj.SpecialityId = model.SpecialityId;
                videoObj.OrderId = model.OrderId;
                videoObj.IsActive = model.IsActive;
                _context.Entry(videoObj).State = EntityState.Modified;
                _context.SaveChanges();
                return videoObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

    }
}
