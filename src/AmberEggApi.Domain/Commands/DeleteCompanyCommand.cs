using System;
using System.ComponentModel.DataAnnotations;
using Api.Common.Cqrs.Core.Commands;

namespace AmberEggApi.Domain.Commands
{
    public class DeleteCompanyCommand : Command
    {
        public DeleteCompanyCommand(Guid id)
        {
            Id = id;
        }

        [Required] public Guid Id { get; protected set; }
    }
}