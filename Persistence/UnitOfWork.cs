using System.Threading.Tasks;
using vega.Core;

namespace vega.Persistence
{
    public class UnitOfWork : IUnifOfWork
    {
        private readonly VegaDbContext context;

        public UnitOfWork(VegaDbContext context)
        {
            this.context = context;
        }
        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }

    }
}