using System;
using System.Collections.Generic;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IUserRetriever
    {
        List<User> GetAllUsers();
        User GetUser(Action<IUserHavingBuilder> having);
    }

    internal class UserRetriever : IUserRetriever
    {
        private readonly ITeamCityCaller m_TeamCityCaller;
        private readonly IUserHavingBuilderFactory m_UserHavingBuilderFactory;

        public UserRetriever(ITeamCityCaller teamCityCaller, IUserHavingBuilderFactory userHavingBuilderFactory)
        {
            m_TeamCityCaller = teamCityCaller;
            m_UserHavingBuilderFactory = userHavingBuilderFactory;
        }

        public List<User> GetAllUsers()
        {
            return m_TeamCityCaller.GetFormat<UserWrapper>("/app/rest/users/").User;
        }

        public User GetUser(Action<IUserHavingBuilder> having)
        {
            var userHavingBuilder = m_UserHavingBuilderFactory.CreateUserHavingBuilder();
            having(userHavingBuilder);

            return m_TeamCityCaller.GetFormat<User>(@"/app/rest/users/{0}", userHavingBuilder.GetLocator());
        }
    }
}