using DoctorConsult.Models;
using DoctorConsult.ViewModels.RequestDocumentVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.Domain.Interfaces
{
    public interface IRequestDocumentRepository
    {
        IEnumerable<IndexRequestDocumentVM> GetAll();
        IndexRequestDocumentVM GetById(int id);
        IEnumerable<IndexRequestDocumentVM> GetRequestDocumentsByRequestTrackingId(int RequestTrackingId);
        void Add(CreateRequestDocumentVM requestDocuments);
        void DeleteRequestDocument(int id);
        RequestDocument GetLastDocumentForRequestTrackingId(int RequestTrackingId);
    }
}
