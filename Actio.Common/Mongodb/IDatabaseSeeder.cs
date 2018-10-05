using System.Threading.Tasks;

namespace Actio.Common.Mongodb
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }
}
