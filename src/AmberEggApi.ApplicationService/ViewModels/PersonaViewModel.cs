using AmberEggApi.Contracts.Entities;
using System;

namespace AmberEggApi.ApplicationService.ViewModels;

public class PersonaViewModel : IViewModel
{
    public string Name { get; set; }

    public Guid Id { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int Version { get; set; }
}