﻿
using DoctorConsult.Models;
using DoctorConsult.ViewModels.RequestTrackingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorConsult.Domain.Interfaces
{
    public interface IRequestTrackingRepository
    {

        RequestTracking GetFirstTrackForRequestByRequestId(int requestId);
        RequestTracking GetLastTrackForRequestByRequestId(int requestId);
        IndexRequestTrackingVM GetAllTrackingsByRequestId(int RequestId, string userId);
        EditRequestTrackingVM GetById(int id);
        int Add(CreateRequestTrackingVM createRequestTracking);
        int Update(EditRequestTrackingVM editRequestTracking);
        void Delete(int id);
    }
}
