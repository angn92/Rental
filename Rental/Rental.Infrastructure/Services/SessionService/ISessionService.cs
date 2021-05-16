using Rental.Core.Domain;

using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.SessionService
{
    interface ISessionService
    {
        Task<Session> CreateSessionAsync(User user);
        Task<Session> GetSessionAsync(string idSession);
        void RemoveSession(string idSession);
    }
}
