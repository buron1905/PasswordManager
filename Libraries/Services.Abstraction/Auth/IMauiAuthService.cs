namespace Services.Abstraction.Auth
{
    public interface IMauiAuthService : IAuthService
    {
        string GetTfaCode();
    }
}
