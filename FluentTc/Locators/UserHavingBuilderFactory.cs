namespace FluentTc.Locators
{
    public interface IUserHavingBuilderFactory
    {
        IUserHavingBuilder CreateUserHavingBuilder();
    }

    public class UserHavingBuilderFactory : IUserHavingBuilderFactory
    {
        public IUserHavingBuilder CreateUserHavingBuilder()
        {
            return new UserHavingBuilder();
        }
    }
}