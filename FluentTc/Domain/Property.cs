namespace FluentTc.Domain
{
    public class Property
    {
        public override string ToString()
        {
            return Name;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}