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
            List<Change> changes = new List<Change>();

            if (buildModel.LastChanges != null)
            {
                changes = buildModel.LastChanges.Change;
            } 
            else if (buildModel.Revisions != null)
            {
                changes = buildModel.Revisions.Revision;
            }

            var buildConfiguration = buildModel.BuildType ?? new BuildConfiguration { Id = buildModel.BuildTypeId };

            return new Build(buildModel.Id, buildModel.Number,
                ConvertBuildStatus(buildModel),
                buildModel.StartDate, buildModel.FinishDate, buildModel.QueuedDate, buildConfiguration,
                buildModel.Agent, changes, buildModel.WebUrl,
                buildModel.Properties, buildModel.TestOccurrences, ConvertBuildState(buildModel));
        }

        private static BuildStatus? ConvertBuildStatus(BuildModel buildModel)
        {
            if (buildModel.Status == null)
                return null;
            BuildStatus result;
            if (!Enum.TryParse(buildModel.Status, true, out result))
                return null;
            return result;
        }

        private static BuildState? ConvertBuildState(BuildModel buildModel)
        {
            if (buildModel.State == null)
                return null;
            BuildState result;
            if (!Enum.TryParse(buildModel.State, true, out result))
                return null;
            return result;
        }
    }
}