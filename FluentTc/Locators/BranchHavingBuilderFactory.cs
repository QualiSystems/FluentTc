namespace FluentTc.Locators
{
    public interface IBranchHavingBuilderFactory
    {
        IBranchHavingBuilder CreateBranchHavingBuilder();
    }

    public class BranchHavingBuilderFactory : IBranchHavingBuilderFactory
    {
        public IBranchHavingBuilder CreateBranchHavingBuilder()
        {
            return new BranchHavingBuilder();
        }
    }
}