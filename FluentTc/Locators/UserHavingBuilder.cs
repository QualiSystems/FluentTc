using System.Collections.Generic;

namespace FluentTc.Locators
{
    public interface IUserHavingBuilder
    {
        UserHavingBuilder InternalUserId(string internalUserId);
        UserHavingBuilder Username(string username);
        string GetLocator();
    }

    public class UserHavingBuilder : IUserHavingBuilder
    {
        readonly List<string> m_Havings = new List<string>();

        public UserHavingBuilder InternalUserId(string internalUserId)
        {
            m_Havings.Add("id:" + internalUserId);
            return this;
        }

        public UserHavingBuilder Username(string username)
        {
            m_Havings.Add("username:" + username);
            return this;
        }

        string IUserHavingBuilder.GetLocator()
        {
            return string.Join(",", m_Havings);
        }
    }
}