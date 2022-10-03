namespace PasswordManager.WebAPI.Services
{
    public interface IIdentityService
    {
        string GenerateJwtToken(string userId, string emailAddress, string secret);
    }
}
