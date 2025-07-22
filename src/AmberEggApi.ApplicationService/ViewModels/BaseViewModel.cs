using System;

namespace AmberEggApi.ApplicationService.ViewModels;

public abstract class BaseViewModel
{
    public Guid Id { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
    public int Version { get; set; }
}