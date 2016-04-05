using System;
using System.Collections.Generic;
using System.Linq;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IBuildModelToBuildConverter
    {
        IList<IBuild> ConvertToBuilds(BuildWrapper buildWrapper);
        IBuild ConvertToBuild(BuildModel buildModel);
    }

    internal class BuildModelToBuildConverter : IBuildModelToBuildConverter
    {
        public IList<IBuild> ConvertToBuilds(BuildWrapper buildWrapper)
        {
            if (int.Parse(buildWrapper.Count) > 0)
            {
                return buildWrapper.Build.Select(ConvertToBuild).ToList();
            }
            return new List<IBuild>();
        }

        public IBuild ConvertToBuild(BuildModel buildModel)
        {
            var changes = buildModel.LastChanges != null ? buildModel.LastChanges.Change : new List<Change>();

            return new Build(buildModel.Id, buildModel.Number,
                ConvertBuildStatus(buildModel),
                buildModel.StartDate, buildModel.FinishDate, buildModel.QueuedDate, buildModel.BuildType,
                buildModel.Agent, changes);
        }

        private static BuildStatus ConvertBuildStatus(BuildModel buildModel)
        {
            return (BuildStatus)Enum.Parse(typeof(BuildStatus), UppercaseFirst(buildModel.Status));
        }

        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
        }
    }
}