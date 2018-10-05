using System.Threading.Tasks;

namespace Actio.Common.Mongodb
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}
