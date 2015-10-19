using System.Collections.Generic;

namespace FluentTc.Locators
{
    public interface IBranchHavingBuilder
    {
        IBranchHavingBuilder Name(string branchName);
        IBranchHavingBuilder Default();
        IBranchHavingBuilder NotDefault();
        IBranchHavingBuilder Unspecified();
        IBranchHavingBuilder NotUnspecified();
        IBranchHavingBuilder Branched();
        IBranchHavingBuilder NotBranched();
        string GetLocator();
    }

    public class BranchHavingBuilder : IBranchHavingBuilder
    {
        private readonly List<string> m_Having = new List<string>();

        public IBranchHavingBuilder Name(string branchName)
        {
            m_Having.Add("name:" + branchName);
            return this;
        }

        public IBranchHavingBuilder Default()
        {
            m_Having.Add("default:" + bool.TrueString);
            return this;
        }

        public IBranchHavingBuilder NotDefault()
        {
            m_Having.Add("default:" + bool.FalseString);
            return this;
        }

        public IBranchHavingBuilder Unspecified()
        {
            m_Having.Add("unspecified:" + bool.TrueString);
            return this;
        }

        public IBranchHavingBuilder NotUnspecified()
        {
            m_Having.Add("unspecified:" + bool.FalseString);
            return this;
        }

        public IBranchHavingBuilder Branched()
        {
            m_Having.Add("branched:" + bool.TrueString);
            return this;
        }

        public IBranchHavingBuilder NotBranched()
        {
            m_Having.Add("branched:" + bool.FalseString);
            return this;
        }

        string IBranchHavingBuilder.GetLocator()
        {
            return string.Join(",", m_Having);
        }
    }
}