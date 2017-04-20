using System.Text.RegularExpressions;

namespace FluentTc.Domain
{
    public class Test
    {
        private static readonly Regex IdRegex = new Regex("^\\/httpAuth\\/app\\/rest/tests\\/id\\:(?<id>\\-?\\d+)$", RegexOptions.Compiled);

        public string Id
        {
            get
            {
                return Test.IdRegex.Match(Href).Groups["id"].Value;
            }
        }

        public string Name { get; set; }

        public string Href { get; set; }
    }
}