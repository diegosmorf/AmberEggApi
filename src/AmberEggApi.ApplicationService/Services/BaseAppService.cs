using Api.Common.Cqrs.Core.Commands;
using AmberEggApi.ApplicationService.Interfaces;
using AutoMapper;

namespace AmberEggApi.ApplicationService.Services
{
    public abstract class BaseAppService : IBaseAppService
    {
        protected readonly ICommandProducer producer;
        protected readonly IMapper mapper;

        protected BaseAppService(ICommandProducer producer, IMapper mapper)
        {
            this.producer = producer;
            this.mapper = mapper;
        }
    }
}