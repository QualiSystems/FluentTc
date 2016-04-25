using System.Collections.Generic;
using FluentTc.Domain;
using System.Linq;

namespace FluentTc.Engine
{
    internal interface IBuildStatisticConverter
    {
        IList<IBuildStatistic> Convert(BuildStatisticsModel buildStatisticsModel);
    }

    internal class BuildStatisticConverter : IBuildStatisticConverter
    {
        public IList<IBuildStatistic> Convert(BuildStatisticsModel buildStatisticsModel)
        {
            if (string.IsNullOrEmpty(buildStatisticsModel.Count) || int.Parse(buildStatisticsModel.Count) <= 0)
            {
                return new List<IBuildStatistic>();
            }
            return buildStatisticsModel.Property.Select(CreateBuildStatistic).ToList();
        }

        private static IBuildStatistic CreateBuildStatistic(Property property)
        {
            return new BuildStatistic(property.Name, property.Value);
        }
    }
}