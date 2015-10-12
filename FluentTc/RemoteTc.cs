using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentTc
{
    public class RemoteTc
    {
        public RemoteTc Connect(Action<TeamCityConfigurationBuilder> connect)
        {
            var teamCityConfigurationBuilder = new TeamCityConfigurationBuilder();
            connect.Invoke(teamCityConfigurationBuilder);
            var teamCityConnectionDetails = teamCityConfigurationBuilder.GetITeamCityConnectionDetails();
            return this;
        }

        public List<Build> GetBuilds(Action<BuildHavingBuilder> having, Action<CountBuilder> count,
            Action<BuildPropertyBuilder> include)
        {
            return new List<Build>();
        }

        public List<Build> GetBuilds(Action<BuildHavingBuilder> having, Action<BuildPropertyBuilder> include)
        {
            return GetBuilds(having, _ => _.All(), include);
        }

        public List<Build> GetBuilds(Action<BuildHavingBuilder> having)
        {
            return GetBuilds(having, _ => _.All(), _ => _.IncludeDefaults());
        }

        public Build GetBuild(Action<BuildHavingBuilder> having, Action<BuildPropertyBuilder> include)
        {
            var builds = GetBuilds(having, include);
            var build = builds.FirstOrDefault();
            if (build == null) throw new BuildNotFoundException();
            return build;
        }

        public BuildConfiguration GetBuildConfiguration(Action<BuildConfigurationHavingBuilder> having, Action<BuildConfigurationPropertyBuilder> include)
        {
            throw new NotImplementedException();
        }

        public IList<BuildConfiguration> GetBuildConfigurations(Action<BuildConfigurationHavingBuilder> having, Action<BuildConfigurationPropertyBuilder> include)
        {
            throw new NotImplementedException();
        }

        public BuildConfiguration SetParameters(Action<BuildConfigurationHavingBuilder> having, Action<BuildParameterValueBuilder> parameters)
        {
            throw new NotImplementedException();
        }

        public BuildConfiguration Run(Action<BuildConfigurationHavingBuilder> having, Action<BuildParameterValueBuilder> parameters)
        {
            throw new NotImplementedException();
        }

        public BuildConfiguration Run(Action<BuildConfigurationHavingBuilder> having, Action<AgentLocatorBuilder> onAgent)
        {
            throw new NotImplementedException();
        }

        public BuildConfiguration Run(Action<BuildConfigurationHavingBuilder> having, Action<AgentLocatorBuilder> onAgent, Action<BuildParameterValueBuilder> parameters)
        {
            throw new NotImplementedException();
        }

        public BuildConfiguration Run(Action<BuildConfigurationHavingBuilder> having)
        {
            throw new NotImplementedException();
        }
        
        public IList<BuildConfiguration> GetBuildConfigurations(Action<BuildProjectHavingBuilder> having, Action<BuildConfigurationPropertyBuilder> include)
        {
            throw new NotImplementedException();
        }

        public BuildConfiguration CreateBuildConfiguration(Action<BuildProjectHavingBuilder> having, string buildConfigurationName)
        {
            throw new NotImplementedException();
        }

        public void AttachToTemplate(Action<BuildConfigurationHavingBuilder> having, Action<BuildTemplateHavingBuilder> templateHaving)
        {
            throw new NotImplementedException();
        }

        public void DeleteBuildConfiguration(Action<BuildConfigurationHavingBuilder> having)
        {
            throw new NotImplementedException();
        }
    }

    public class BuildTemplateHavingBuilder
    {
        public BuildTemplateHavingBuilder TemplateName(string templateName)
        {
            return this;
        }
    }

    public class AgentLocatorBuilder
    {
        public AgentLocatorBuilder AgentName(string agentName)
        {
            return this;
        }

        public AgentLocatorBuilder AgentId(string agentId)
        {
            return this;
        }
    }

    public class BuildParameterValueBuilder
    {
        public BuildParameterValueBuilder Parameters(string name, string value)
        {
            return this;
        }
    }

    public class BuildParametersBuilder
    {
    }

    public class BuildProjectHavingBuilder
    {
        public BuildProjectHavingBuilder ProjectId(string projectId)
        {
            return this;
        }
    }

    public class BuildConfiguration
    {
    }

    public class BuildConfigurationPropertyBuilder
    {
        public BuildConfigurationPropertyBuilder IncludeDefaults()
        {
            return this;
        }
    }

    public class BuildNotFoundException : Exception
    {
    }

    public class CountBuilder
    {
        public CountBuilder All()
        {
            return this;
        }

        public CountBuilder Top(int count)
        {
            return this;
        }
    }

    public class TeamCityConfigurationBuilder
    {
        private string m_Password;
        private string m_TeamCityHost;
        private string m_Username;

        public TeamCityConfigurationBuilder()
        {
            m_TeamCityHost = "localhost";
            m_Username = "guest";
            m_Password = string.Empty;
        }

        public TeamCityConfigurationBuilder ToHost(string teamCityHost)
        {
            m_TeamCityHost = teamCityHost;
            return this;
        }

        public TeamCityConfigurationBuilder AsGuest()
        {
            m_Username = "guest";
            return this;
        }

        public TeamCityConfigurationBuilder AsUser(string username, string password)
        {
            m_Username = username;
            m_Password = password;
            return this;
        }

        internal ITeamCityConnectionDetails GetITeamCityConnectionDetails()
        {
            return new TeamCityConnectionDetails(m_TeamCityHost, m_Username, m_Password);
        }
    }

    internal interface ITeamCityConnectionDetails
    {
        string TeamCityHost { get; }
        string Username { get; }
        string Password { get; }
    }

    internal class TeamCityConnectionDetails : ITeamCityConnectionDetails
    {
        private readonly string m_Password;
        private readonly string m_TeamCityHost;
        private readonly string m_Username;

        public TeamCityConnectionDetails(string teamCityHost, string username, string password)
        {
            m_TeamCityHost = teamCityHost;
            m_Username = username;
            m_Password = password;
        }

        public string TeamCityHost
        {
            get { return m_TeamCityHost; }
        }

        public string Username
        {
            get { return m_Username; }
        }

        public string Password
        {
            get { return m_Password; }
        }
    }

    public class BuildPropertyBuilder
    {
        public BuildPropertyBuilder IncludeDefaults()
        {
            return this;
        }
    }

    public class BuildHavingBuilder
    {
        private readonly List<string> m_Having = new List<string>();

        public BuildHavingBuilder Personal()
        {
            m_Having.Add("personal:" + bool.TrueString);
            return this;
        }

        public BuildHavingBuilder BelongingToBuildConfiguration(
            Action<BuildConfigurationHavingBuilder> havingBuildConfig)
        {
            var buildConfigurationHavingBuilder = new BuildConfigurationHavingBuilder();
            havingBuildConfig.Invoke(buildConfigurationHavingBuilder);
            m_Having.AddRange(buildConfigurationHavingBuilder.Get());
            return this;
        }

        public BuildHavingBuilder HavingId(int buildId)
        {
            m_Having.Add("id:" + buildId);
            return this;
        }
    }

    public class BuildConfigurationHavingBuilder
    {
        public BuildConfigurationHavingBuilder ConfigurationId(int buildConfigurationId)
        {
            return this;
        }

        public BuildConfigurationHavingBuilder ConfigurationName(string buildConfigurationName)
        {
            return this;
        }

        public IEnumerable<string> Get()
        {
            return new string[] {};
        }
    }

    public class Build
    {
    }
}