﻿using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IClinicService
    {
        IEnumerable<Clinic> GetAllClinic();
    }
}
