using System.Security.Claims;

namespace ToDoMinimalAPI.Auth
{
    public interface IUserContextService
    {
        public ClaimsPrincipal? User { get; }
        public int? GetUserId { get; }
    }

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal? User => _httpContextAccessor?.HttpContext?.User;

        public int? GetUserId =>
            User is null ? null : (int)int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
