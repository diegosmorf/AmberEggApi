using AmberEggApi.ApplicationService.Contracts;
using AmberEggApi.ApplicationService.Mapping;
using AmberEggApi.Contracts.Entities;
using AmberEggApi.Contracts.Repositories;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmberEggApi.ApplicationService.QueryHandlers;

public class GenericQueryHandler<TDomainEntity, TViewlModel>(IMapper mapper, IRepository<TDomainEntity> repository) :
    IQueryHandler<TViewlModel>
        where TDomainEntity : class, IDomainEntity, new()
        where TViewlModel : class, IViewModel, new()
{
    protected readonly IRepository<TDomainEntity> repository = repository;
    protected readonly IMapper mapper = mapper;

    public async Task<IEnumerable<TViewlModel>> GetAll(CancellationToken cancellationToken)
    {
        var list = await repository.ListAll(cancellationToken);

        return mapper.MapList<TDomainEntity, TViewlModel>(list);
    }
    public async Task<TViewlModel> Get(Guid id, CancellationToken cancellationToken)
    {
        var instance = await repository.SearchById(id, cancellationToken);

        return mapper.Map<TDomainEntity, TViewlModel>(instance);
    }
}