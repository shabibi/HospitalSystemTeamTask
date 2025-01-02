﻿using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{


    public interface IBranchService
    {
        void AddBranch(BranchDTO branchDto);
        IEnumerable<Branch> GetAllBranches();

        Branch GetBranchDetailsByBranchName(string branchName);
        void UpdateBranch(string branchName, UpdateBranchDTO updatedBranchDto);
        void SetBranchStatus(string branchName, bool isActive);
        BranchDTO GetBranchById(int id);
        string GetBranchName(int branchId);
    }

    
}