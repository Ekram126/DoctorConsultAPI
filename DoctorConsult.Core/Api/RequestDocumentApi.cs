
using DoctorConsult.Domain.Interfaces;
using DoctorConsult.Models;
using DoctorConsult.ViewModels.RequestDocumentVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.Core.Repositories
{
    public class RequestDocumentApi : IRequestDocumentRepository
    {
        private readonly ApplicationDbContext _context;
        private string? msg;

        public RequestDocumentApi(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds the specified request documents.
        /// </summary>
        /// <param name="requestDocuments">The request documents.</param>
        public void Add(CreateRequestDocumentVM requestDocuments)
        {
            try
            {
                if (requestDocuments != null)
                {

                    RequestDocument requestDocument = new RequestDocument();
                    requestDocument.FileName = requestDocuments.FileName;
                    requestDocument.Title = requestDocuments.Title;
                    requestDocument.RequestTrackingId = int.Parse(requestDocuments.RequestTrackingId.ToString());
                    _context.Add(requestDocument);
                    _context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        /// <summary>
        /// Delete the request document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void DeleteRequestDocument(int id)
        {
            RequestDocument requestDocument = _context.RequestDocuments.Find(id);
            try
            {
                if (requestDocument != null)
                {
                    var folderName = Path.Combine("UploadedAttachments", "RequestDocuments");
                    var folderPathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var filePath = folderPathToSave + "/" + requestDocument.FileName;
                    FileInfo file = new FileInfo(filePath);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    _context.RequestDocuments.Remove(requestDocument);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        /// <summary>
        /// Gets all Request documents.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IndexRequestDocumentVM> GetAll()
        {
            return _context.RequestDocuments.Include(r => r.RequestTracking.Request).Select(req => new IndexRequestDocumentVM
            {
                Id = req.Id,
                FileName = req.FileName,
                Title = req.Title,
                RequestTrackingId = req.RequestTrackingId,
            }).ToList();
        }


        /// <summary>
        /// Get  Request document the by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public IndexRequestDocumentVM GetById(int id)
        {
            return _context.RequestDocuments.Where(r => r.Id == id).Include(r => r.RequestTracking.Request).Select(req => new IndexRequestDocumentVM
            {
                Id = req.Id,
                FileName = req.FileName,
                Title = req.Title,
                RequestTrackingId = req.RequestTrackingId,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Gets the last document for request tracking id.
        /// </summary>
        /// <param name="RequestTrackingId">The request tracking identifier.</param>
        /// <returns></returns>
        public RequestDocument GetLastDocumentForRequestTrackingId(int RequestTrackingId)
        {
            RequestDocument documentObj = new RequestDocument();
            var lstDocuments = _context.RequestDocuments.Where(a => a.RequestTrackingId == RequestTrackingId).OrderBy(a => a.FileName).ToList();
            if (lstDocuments.Count > 0)
            {
                documentObj = lstDocuments.Last();
            }
            return documentObj;
        }


        /// <summary>
        /// Gets the request documents by request tracking id.
        /// </summary>
        /// <param name="RequestTrackingId">The request tracking identifier.</param>
        /// <returns></returns>
        public IEnumerable<IndexRequestDocumentVM> GetRequestDocumentsByRequestTrackingId(int RequestTrackingId)
        {
            return _context.RequestDocuments.Include(r => r.RequestTracking.Request).Where(req => req.RequestTrackingId == RequestTrackingId).OrderBy(a => a.FileName).Select(req => new IndexRequestDocumentVM
            {
                Id = req.Id,
                FileName = req.FileName,
                Title = req.Title,
                RequestTrackingId = req.RequestTrackingId,
                //Subject = req.RequestTracking != null? req.RequestTracking.Request.Subject:""
            }).ToList();
        }
    }
}


