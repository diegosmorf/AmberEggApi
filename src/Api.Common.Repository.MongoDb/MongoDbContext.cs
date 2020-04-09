using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Common.Repository.MongoDb
{
    public sealed class MongoDbContext : IMongoDbContext
    {
        private readonly List<Func<Task>> commands;
        private readonly IMongoDatabase database;

        public MongoDbContext(MongoSettings settings)
        {
            BsonDefaults.GuidRepresentation = GuidRepresentation.CSharpLegacy;
            commands = new List<Func<Task>>();
            RegisterConventions();

            MongoClient = new MongoClient(settings.ConnectionString);
            database = MongoClient.GetDatabase(settings.DatabaseName);
        }

        private MongoClient MongoClient { get; }

        private IClientSessionHandle Session { get; set; }

        public async Task SaveChanges()
        {
            using (Session = await MongoClient.StartSessionAsync())
            {
                Session.StartTransaction();

                var commandTasks = commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await Session.CommitTransactionAsync();
            }

            commands.Clear();
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return database.GetCollection<T>(name);
        }

        public void Dispose()
        {
            // Cleanup
            Session = null;
        }

        public async Task AddCommand(Func<Task> func)
        {
            await Task.Run(() => commands.Add(func));
        }

        private void RegisterConventions()
        {
            var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfDefaultConvention(true)
            };

            ConventionRegistry.Register("CustomConventions", pack, t => true);
        }
    }
}