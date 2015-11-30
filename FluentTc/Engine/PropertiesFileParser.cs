using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text.RegularExpressions;

namespace FluentTc.Engine
{
    internal interface IPropertiesFileParser
    {
        Dictionary<string, string> ParsePropertiesFile(string teamCityBuildPropertiesFile);
    }

    internal class PropertiesFileParser : IPropertiesFileParser
    {
        private readonly IFileSystem m_FileSystem;
        private static readonly Regex ParsingRegex = new Regex(@"^(?<Name>(\w+\.)*\w+)=(?<Value>.*)$", RegexOptions.Compiled | RegexOptions.Multiline);

        public PropertiesFileParser(IFileSystem fileSystem)
        {
            m_FileSystem = fileSystem;
        }

        public Dictionary<string, string> ParsePropertiesFile(string teamCityBuildPropertiesFile)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var allLines = m_FileSystem.File.ReadAllLines(teamCityBuildPropertiesFile);
            foreach (var line in allLines)
            {
                var match = ParsingRegex.Match(line);
                if (!match.Success) continue;
                parameters.Add(match.Groups["Name"].Value, DecodeValue(match.Groups["Value"].Value));
            }
            return parameters;
        }

        private static string DecodeValue(string parameterValue)
        {
            return parameterValue.Replace(@"\:",@":").Replace(@"\\", @"\").Replace(@"\=", @"=");
        }
    }
}