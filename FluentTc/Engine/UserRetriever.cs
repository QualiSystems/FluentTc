using System.Collections.Generic;
using FluentTc.Domain;

namespace FluentTc.Engine
{
    internal interface IUserRetriever
    {
        List<User> GetAllUsers();
    }

    internal class UserRetriever : IUserRetriever
    {
        private readonly ITeamCityCaller m_TeamCityCaller;

        public UserRetriever(ITeamCityCaller teamCityCaller)
        {
            m_TeamCityCaller = teamCityCaller;
        }

        public List<User> GetAllUsers()
        {
            return m_TeamCityCaller.GetFormat<UserWrapper>("/app/rest/users/").User;
        }
    }
}