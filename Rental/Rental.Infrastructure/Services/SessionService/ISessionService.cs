using JetBrains.Annotations;
using Rental.Core.Domain;
using Rental.Core.Enum;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.SessionService
{
    public interface ISessionService : IService
    {
        Task<Session> CreateSession([NotNull] Customer username);
        Task RemoveSession([NotNull] int session);
        void RemoveAllSession([NotNull] string username);
        Task ChangeSessionStatus([NotNull] int sessionId, [NotNull] SessionState sessionState);
    }
}
