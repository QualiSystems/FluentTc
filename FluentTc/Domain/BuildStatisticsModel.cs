using System.Collections.Generic;

namespace FluentTc.Domain
{
    public class BuildStatisticsModel
    {
        public string Count { get; set; }
        public List<Property> Property { get; set; }
    }
}