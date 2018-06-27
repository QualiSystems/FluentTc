using System;

namespace FluentTc.Domain
{
    public class Change
    {
        public string Username { get; set; }
        public string WebUrl { get; set; }
        public string Href { get; set; }
        public long Id { get; set; }
        public string Version { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public string VcsBranchName { get; set; }
        public FileWrapper Files { get; set; }

        public User User { get; set; }
    }
}