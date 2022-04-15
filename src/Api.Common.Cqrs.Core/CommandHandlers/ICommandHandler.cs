﻿using Api.Common.Cqrs.Core.Commands;
using Api.Common.Cqrs.Core.Entities;
using System.Threading.Tasks;

namespace Api.Common.Cqrs.Core.CommandHandlers
{
    public interface ICommandHandler<in TCommand, TDomainEntity>
        where TCommand : ICommand
        where TDomainEntity : IAggregateRoot
    {
        Task<TDomainEntity> Handle(TCommand command);
    }
}