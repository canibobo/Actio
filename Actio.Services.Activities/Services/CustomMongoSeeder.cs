using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Actio.Common.Mongodb;
using Actio.Services.Activities.Domain.Models;
using Actio.Services.Activities.Domain.Repositories;
using MongoDB.Driver;

namespace Actio.Services.Activities.Services
{
    public class CustomMongoSeeder : MongoSeeder
    {
        private readonly ICategoryRepo _categoryRepository;

        public CustomMongoSeeder(IMongoDatabase database,
            ICategoryRepo categoryRepository)
            : base(database)
        {
            _categoryRepository = categoryRepository;
        }

        protected override async Task CustomSeedAsync()
        {
            var categories = new List<string>
            {
                "work",
                "sport",
                "hobby"
            };
            await Task.WhenAll(categories.Select(x => _categoryRepository
                .AddAsync(new Category(x))));
        }
    }
}
