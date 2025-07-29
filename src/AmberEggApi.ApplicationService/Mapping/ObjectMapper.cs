using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AmberEggApi.ApplicationService.Mapping;

public class ObjectMapper : IMapper
{
    // Updates an existing target instance with values from source (public properties, same name)
    public TTargetModel Map<TSourceModel, TTargetModel>(TSourceModel source, TTargetModel target)
        where TSourceModel : class
        where TTargetModel : class
    {
        if (source == null || target == null)
            return null;
        MapProperties(source, target);
        return target;
    }

    // Creates a new target instance and maps values from source
    public TTargetModel Map<TSourceModel, TTargetModel>(TSourceModel source)
        where TSourceModel : class
        where TTargetModel : class, new()
    {
        if (source == null)
            return null;
        var target = new TTargetModel();
        MapProperties(source, target);
        return target;
    }

    // Maps a collection of source objects to a list of target objects
    public IEnumerable<TTargetModel> MapList<TSourceModel, TTargetModel>(IEnumerable<TSourceModel> sourceList)
        where TSourceModel : class
        where TTargetModel : class, new()
    {
        if (sourceList == null)
            return Enumerable.Empty<TTargetModel>();
        return sourceList.Select(source => Map<TSourceModel, TTargetModel>(source)).ToList();
    }

    // Helper: maps public properties with the same name from source to target
    private static void MapProperties<TSourceModel, TTargetModel>(TSourceModel source, TTargetModel target)
        where TSourceModel : class
        where TTargetModel : class
    {
        var sourceProps = typeof(TSourceModel).GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var targetProps = typeof(TTargetModel).GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (var sProp in sourceProps)
        {
            if (!sProp.CanRead)
                continue;
            var tProp = targetProps.FirstOrDefault(p => p.Name == sProp.Name && p.CanWrite);
            if (tProp != null && tProp.PropertyType.IsAssignableFrom(sProp.PropertyType))
            {
                var value = sProp.GetValue(source);
                tProp.SetValue(target, value);
            }
        }
    }
}
