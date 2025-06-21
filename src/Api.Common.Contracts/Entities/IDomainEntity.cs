using System;

namespace Api.Common.Contracts.Entities;
public interface IDomainEntity :
    IEntityWithPrimaryKey<Guid>,
    IEntityWithAudit
{
}