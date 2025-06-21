namespace Api.Common.Contracts.Entities;
public interface IEntityWithPrimaryKey<TId>
{
    TId Id { get; set; }
}