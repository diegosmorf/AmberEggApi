using System.Collections.Generic;

namespace AmberEggApi.ApplicationService.Mapping;

public interface IMapper
{
    TTargetModel Map<TSourceModel, TTargetModel>(TSourceModel source, TTargetModel target)
        where TSourceModel : class
        where TTargetModel : class;

    TTargetModel Map<TSourceModel, TTargetModel>(TSourceModel source)
        where TSourceModel : class
        where TTargetModel : class, new();

    IEnumerable<TTargetModel> MapList<TSourceModel, TTargetModel>(IEnumerable<TSourceModel> sourceList)
        where TSourceModel : class
        where TTargetModel : class, new();
}