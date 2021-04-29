using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Password
{
    public class ChangePasswordHandler : ICommandHandler<ChangePassword>
    {
        private readonly RentalContext _rentalContext;

        public ChangePasswordHandler(RentalContext rentalContext)
        {
            _rentalContext = rentalContext;
        }

        public async Task HandleAsync(ChangePassword command)
        {
            
        }
    }
}
