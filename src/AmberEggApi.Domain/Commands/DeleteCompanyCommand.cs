using Api.Common.Cqrs.Core.Commands;
using System;
using System.ComponentModel.DataAnnotations;

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