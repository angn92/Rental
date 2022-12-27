using JetBrains.Annotations;
using Rental.Core.Domain;
using Rental.Core.Enum;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.SessionService
{
    public interface ISessionService : IService
    {
        /// <summary>
        /// Create new session for customer
        /// </summary>
        /// <param name="username"></param>
        /// <returns>New customer session</returns>
        Task<Session> CreateSession([NotNull] Customer username);

        /// <summary>
        /// Get session with ID
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns>Customer session</returns>
        Task<Session> GetSessionAsync([NotNull] int sessionId);

        /// <summary>
        /// Remove session with specified ID 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        Task RemoveSession([NotNull] int session);

        // Remove all session for given customer
        void RemoveAllSession([NotNull] string username);

        /// <summary>
        /// Change current session state
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="sessionState"></param>
        /// <returns></returns>
        Task ChangeSessionStatus([NotNull] int sessionId, [NotNull] SessionState sessionState);
    }
}
