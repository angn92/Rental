using JetBrains.Annotations;
using Rental.Core.Domain;
using Rental.Infrastructure.EF;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.SessionService
{
    public interface ISessionService : IService
    {
        Task<Session> GetSessionAsync([NotNull] int sessionId);

        // Remove session about given id.
        Task RemoveSession([NotNull] int session);

        // Remove all sessions for given customer if any exists. 
        Task RemoveAllSession([NotNull] string username);

        Task<Session> CreateNotAuthorizeSession([NotNull] Customer username);
    }
}
