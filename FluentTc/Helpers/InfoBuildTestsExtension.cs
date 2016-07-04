using System;
using System.Text.RegularExpressions;
using FluentTc.Domain;

namespace FluentTc.Helpers
{
    public static class InfoBuildTestsExtension
    {
        public static TestInfo GetTestInfo(this Build build)
        {
            if (build == null)
                throw new ArgumentNullException("build");

            var statusText = build.StatusText;
            const string testsInfoPrefix = "Tests ";
            if (statusText == null || !statusText.StartsWith(testsInfoPrefix))
                return null;
            statusText = statusText.Substring(testsInfoPrefix.Length);

            var statusSplit = statusText.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
            var result = new TestInfo();
            foreach (var item in statusSplit)
            {
                var testRecordInfo = ParseTestRecord(item.Trim());
                if (testRecordInfo != null)
                {
                    switch (testRecordInfo.Name)
                    {
                        case "passed":
                            result.Passed = testRecordInfo.Count;
                            break;
                        case "failed":
                            result.Failed = testRecordInfo.Count;
                            result.FailedNew = testRecordInfo.CountNew;
                            break;
                        case "muted":
                            result.Muted = testRecordInfo.Count;
                            break;
                    }

                }
            }
            return result;
        }

        private static readonly Lazy<Regex> TestRecordRegex = new Lazy<Regex>(
            () => new Regex(@"(?<name>\w+): (?<count>\d+)( \((?<countNew>\d+) new\))?", RegexOptions.Compiled));
        private static TestRecordInfo ParseTestRecord(string testSplit)
        {
            var match = TestRecordRegex.Value.Match(testSplit);
            if (!match.Success)
                return null;
            return new TestRecordInfo
            {
                Name = match.Groups["name"].Value.ToLower(),
                Count = int.Parse(match.Groups["count"].Value),
                CountNew = int.Parse(match.Groups["countNew"].Value)
            };
        }

        private class TestRecordInfo
        {
            public string Name { get; set; }
            public int Count { get; set; }
            public int CountNew { get; set; }
        }
    }
}
