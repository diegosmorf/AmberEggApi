using AmberEggApi.Domain.Models;
using Api.Common.Cqrs.Core.Events;
using System;

namespace AmberEggApi.Domain.Events
{
    public class CompanyUpdatedEvent : Event
    {
        public CompanyUpdatedEvent(Company company, Guid commandMessageId) : base(commandMessageId)
        {
            Company = company;
        }

        public Company Company { get; }
    }
}