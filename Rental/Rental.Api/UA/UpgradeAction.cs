using Rental.Infrastructure.EF;
using System.Linq;

namespace Rental.Api.UA
{
    public interface IUpgrade
    {
        void Upgrade(string upgradeVersion, string description);
    }
    public class UpgradeAction : IUpgrade
    {
        private readonly ApplicationDbContext context;

        public UpgradeAction(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Upgrade(string upgradeVersion, string description)
        {
            throw new System.NotImplementedException();
        }
    }
}
