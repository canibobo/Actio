﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Actio.Common.Mongodb
{
    public class MongoInitializer : IDatabaseInitializer
    {
        private readonly IMongoDatabase _database;
        private readonly IDatabaseSeeder _seeder;
        private bool _initialized;
        private readonly bool _seed;

        public MongoInitializer(IMongoDatabase database, IOptions<MongoOptions> options, IDatabaseSeeder seeder)
        {
            _database = database;
            _seeder = seeder;
            _seed = options.Value.Seed;
        }
        public async Task InitializeAsync()
        {
            if (_initialized)
                return;

            RegisterConventions();

            _initialized = true;
            if (!_seed)
                return;

            await _seeder.SeedAsync();
        }

        private void RegisterConventions()
        {
            ConventionRegistry.Register("ActioConventions", new MongoConvention(), x => true);
        }

        private class MongoConvention : IConventionPack
        {
            public IEnumerable<IConvention> Conventions => new List<IConvention>
            {
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String),
                new CamelCaseElementNameConvention()
            };
        }
    }
}
