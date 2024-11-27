using DoctorConsult.Models;
using DoctorConsult.ViewModels.RequestVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.Domain.Interfaces
{
    public interface IRequestRepository
    {

        Task<IndexRequestVM> ListRequests(SortAndFilterRequestVM data, int pageNumber, int pageSize);
        ViewRequestVM GetById(int id);
        int Add(CreateRequestVM createRequestVM);
        void Update(EditRequestVM editRequestVM);
        void Delete(int id);
        GeneratedRequestNumberVM GenerateRequestNumber(); 
    }
}
