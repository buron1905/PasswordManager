namespace PasswordManager.WebAPI.Features.Identity.Services
{
    public interface IIdentityService
    {
        string GenerateJwtToken(string userId, string emailAddress, string secret);
    }
}
