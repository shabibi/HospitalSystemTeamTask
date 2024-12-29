﻿using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IClinicService
    {
        IEnumerable<Clinic> GetAllClinic();
        void AddClinic(Clinic clinic);
        Clinic GetClinicById(int Cid);
    }
}
