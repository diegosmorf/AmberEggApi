using System;

namespace Api.Common.Contracts.Entities;

public interface IDomainEntity
{
    Guid Id { get; }
    DateTime CreateDate { get; }
    DateTime? ModifiedDate { get;}
    int Version { get; }
    void Create();
    void Update();
}