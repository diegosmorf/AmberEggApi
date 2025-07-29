using AmberEggApi.Contracts.Entities;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.ApplicationService.Contracts;

public interface IQueryHandler<TViewlModel>    
    where TViewlModel : class, IViewModel
{
    Task<IEnumerable<TViewlModel>> GetAll(CancellationToken cancellationToken);
    Task<TViewlModel> Get(Guid id, CancellationToken cancellationToken);   
}