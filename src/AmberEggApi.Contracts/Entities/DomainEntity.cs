using System;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Contracts.Entities;

public abstract class DomainEntity : IDomainEntity
{

    [Key]
    public Guid Id { get; private set; }

    [Required]
    public DateTime CreateDate { get; private set; }

    public DateTime? ModifiedDate { get; private set; }

    public int Version { get; private set; }

    public override string ToString()
    {
        return $"Type:{GetType().Name} - Id:{Id}";
    }

    private void UpdateVersion()
    {        
        Version++;
    }

    public void Create()
    {
        Id = Guid.NewGuid();
        CreateDate = DateTime.UtcNow;
        UpdateVersion();
    }

    public void Update()
    {
        ModifiedDate = DateTime.UtcNow;
        UpdateVersion();
    }
}