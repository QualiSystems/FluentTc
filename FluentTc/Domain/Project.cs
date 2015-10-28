namespace FluentTc.Domain
{
    public class Project
    {
        public override string ToString()
        {
            return Name;
        }

        public bool Archived { get; set; }
        public string Description { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string WebUrl { get; set; }

        public BuildTypeWrapper BuildTypes { get; set; }
        public ProjectWrapper Projects { get; set; }
        public Parameters Parameters { get; set; }
        public bool IsRoot { get { return !string.IsNullOrEmpty(Id) && Id.Equals("_Root"); } }
    }
}