using System.Collections.Generic;

namespace FluentTc.Domain
{
    public class Parameters
    {
        public override string ToString()
        {
            return "parameters";
        }

        public List<Property> Property { get; set; }
    }
}