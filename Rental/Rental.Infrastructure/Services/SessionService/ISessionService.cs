using Rental.Core.Domain;

using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.SessionService
{
    public interface ISessionService
    {
        Task<Session> CreateSessionAsync(Customer user);
        Task<Session> GetSessionAsync(int idSession);
        Task RemoveSession(int Session);
    }
}
