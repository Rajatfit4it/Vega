using System.Threading.Tasks;

namespace vega.Core
{
    public interface IUnifOfWork
    {
        Task CompleteAsync();
    }
}