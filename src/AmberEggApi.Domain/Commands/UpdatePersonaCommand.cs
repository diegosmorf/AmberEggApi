﻿using AmberEggApi.Contracts.Commands;

using System;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Domain.Commands;
public class UpdatePersonaCommand(Guid id, string name) : IUpdateCommand
{
    [Required]
    public Guid Id { get; } = id;

    [MinLength(2)]
    [MaxLength(20)]
    [Required]
    public string Name { get; set; } = name;
}