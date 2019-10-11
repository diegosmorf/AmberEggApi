using AmberEggApi.Domain.Models;
using MongoDB.Bson.Serialization;

namespace AmberEggApi.Infrastructure.Repositories
{
    public static class CompanyMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Company>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.Name).SetIsRequired(true);
            });
        }
    }
}