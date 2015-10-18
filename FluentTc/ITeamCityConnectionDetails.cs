namespace FluentTc
{
    internal interface ITeamCityConnectionDetails
    {
        string TeamCityHost { get; }
        string Username { get; }
        string Password { get; }
        bool ActAsGuest { get; }
        bool UseSSL { get; }
    }
}