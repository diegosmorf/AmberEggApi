using AmberEggApi.Contracts.Commands;

using System;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.Commands;
public class DeletePersonaCommand(Guid id) : IDeleteCommand
{
    [Required]
    public Guid Id { get; set; } = id;
}