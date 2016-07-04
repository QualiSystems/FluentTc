using System;
using System.Collections.Generic;
using FluentTc.Locators;

namespace FluentTc.Domain
{
    public interface IBuild
    {
        long Id { get; }
        string Number { get; }
        BuildStatus? Status { get; }
        DateTime StartDate { get; }
        DateTime FinishDate { get; }
        DateTime QueuedDate { get; }
        BuildConfiguration BuildConfiguration { get; }
        Agent Agent { get; }
        List<Change> Changes { get; }
        string WebUrl { get; }
        string StatusText { get; }
        void SetChanges(List<Change> changes);
        void SetBuildConfiguration(BuildConfiguration buildConfiguration);
    }

    public class Build : IBuild
    {
        private readonly Agent m_Agent;
        private BuildConfiguration m_BuildConfiguration;
        private readonly List<Change> m_Changes;
        private readonly DateTime m_FinishDate;
        private readonly long m_Id;
        private readonly string m_Number;
        private readonly DateTime m_QueuedDate;
        private readonly DateTime m_StartDate;
        private readonly BuildStatus? m_Status;
        private readonly string m_WebUrl;
        private readonly string m_StatusText;

        public Build(long id, string number, BuildStatus? status, DateTime startDate, DateTime finishDate,
            DateTime queuedDate, BuildConfiguration buildConfiguration, Agent agent, List<Change> changes, string webUrl,
            string statusText)
        {
            m_Id = id;
            m_Number = number;
            m_Status = status;
            m_StartDate = startDate;
            m_FinishDate = finishDate;
            m_QueuedDate = queuedDate;
            m_BuildConfiguration = buildConfiguration;
            m_Agent = agent;
            m_Changes = changes;
            m_WebUrl = webUrl;
            m_StatusText = statusText;
        }

        public long Id
        {
            get { return m_Id; }
        }

        public string Number
        {
            get { return m_Number; }
        }

        public BuildStatus? Status
        {
            get { return m_Status; }
        }

        public DateTime StartDate
        {
            get { return m_StartDate; }
        }

        public DateTime FinishDate
        {
            get { return m_FinishDate; }
        }

        public DateTime QueuedDate
        {
            get { return m_QueuedDate; }
        }

        public BuildConfiguration BuildConfiguration
        {
            get { return m_BuildConfiguration; }
        }

        public Agent Agent
        {
            get { return m_Agent; }
        }

        public List<Change> Changes
        {
            get { return m_Changes; }
        }

        public string WebUrl
        {
            get { return m_WebUrl; }
        }

        public string StatusText
        {
            get { return m_StatusText; }
        }

        public void SetChanges(List<Change> changes)
        {
            m_Changes.Clear();
            m_Changes.AddRange(changes);
        }

        public void SetBuildConfiguration(BuildConfiguration buildConfiguration)
        {
            m_BuildConfiguration = buildConfiguration;
        }
    }
}