namespace Services.Abstraction.Auth
{
    public interface IMauiAuthService : IAuthService
    {
        Task<string> GetTfaCode();
    }
}
