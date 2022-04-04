using JetBrains.Annotations;
using Rental.Core.Domain;
using Rental.Infrastructure.EF;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.SessionService
{
    public interface ISessionService : IService
    {
        Task<Session> CreateSessionAsync([NotNull] ApplicationDbContext context, [NotNull]  Customer user);
        Task<Session> GetSessionAsync([NotNull] ApplicationDbContext context, [NotNull] int idSession);

        // Remove session about given id.
        Task RemoveSession([NotNull] ApplicationDbContext context, [NotNull] int session);

        // Remove all sessions for given customer if any exists. 
        Task RemoveAllSession([NotNull] ApplicationDbContext context, [NotNull] string username);
    }
}
