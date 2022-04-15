using System;

namespace Api.Common.Contracts.Entities
{
    public interface IEntityWithAudit
    {
        DateTime CreateDate { get; set; }
        DateTime? ModifiedDate { get; set; }
    }
}