using System;
using Api.Common.Cqrs.Core.Events;
using AmberEggApi.Domain.Models;

namespace AmberEggApi.Domain.Events
{
    public class CompanyDeletedEvent : Event
    {
        public CompanyDeletedEvent(Company company, Guid commandMessageId) : base(commandMessageId)
        {
            Company = company;
        }

        public Company Company { get; }
    }
}