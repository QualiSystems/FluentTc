using System.Collections.Generic;

namespace FluentTc.Domain
{
    public class Properties
    {
        public override string ToString()
        {
            return "properties";
        }
        public List<Property> Property { get; set; }
    }
}