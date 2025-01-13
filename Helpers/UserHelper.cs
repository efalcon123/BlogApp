using System.Security.Claims;

namespace BlogApp.Helpers
{
    public class UserHelper
    {
        private readonly ClaimsPrincipal _user;

        public UserHelper(ClaimsPrincipal user)
        {
            _user = user;
        }

        public Guid? GetCurrentUserId()
        {
            var userId = _user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : null;
        }

        public string? GetUserRole()
        {
            return _user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        }

        public bool IsAdmin()
        {
            return GetUserRole() == "Admin";
        }
    }
}
