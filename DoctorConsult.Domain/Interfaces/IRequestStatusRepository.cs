using DoctorConsult.Models;
using DoctorConsult.ViewModels.RequestStatusVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.Domain.Interfaces
{
    public interface IRequestStatusRepository
    {
        IEnumerable<IndexRequestStatusVM.GetData> GetAll();
        RequestStatus GetById(int id);
        int Add(RequestStatus requestStatusObj);
        int Update(RequestStatus requestStatusObj);
        int Delete(int id);
       IndexRequestStatusVM GetRequestStatusByUserId(string userId);

        IndexRequestStatusVM GetRequestStatusByUserIdAndSpecialityId(string userId,int specialityId);
    }
}
