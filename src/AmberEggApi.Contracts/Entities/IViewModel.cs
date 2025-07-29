using System;

namespace AmberEggApi.Contracts.Entities;

public interface IViewModel
{
    Guid Id { get; set; }
    DateTime CreateDate { get; set; }
    DateTime? ModifiedDate { get; set; }
    int Version { get; set; }
}

