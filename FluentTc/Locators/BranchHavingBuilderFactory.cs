namespace FluentTc.Locators
{
    public interface IBranchHavingBuilderFactory
    {
        BranchHavingBuilder CreateBranchHavingBuilder();
    }

    public class BranchHavingBuilderFactory : IBranchHavingBuilderFactory
    {
        public BranchHavingBuilder CreateBranchHavingBuilder()
        {
            return new BranchHavingBuilder();
        }
    }
}