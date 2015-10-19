using System.Collections.Generic;

namespace FluentTc.Domain
{
    public class VcsRootEntries
    {
        public override string ToString()
        {
            return "vcs-root-entries";
        }

        public List<VcsRootEntry> VcsRootEntry { get; set; }
    }
}