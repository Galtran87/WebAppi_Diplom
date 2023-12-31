﻿using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using WebApi_HW7;

namespace WebAppi_Diplom
{
    public interface ITeamService
    {
        IEnumerable<string> GetAllTeams();
        void AddTeam(string team);
        void UpdateTeam(int id, string updatedTeam);
        void DeleteTeam(int id);
        string GetTeam(int id);
        Team GetTeamWithPlayers(int id);
        void AddPlayerToTeam(int id, string playerName);
    }
}
