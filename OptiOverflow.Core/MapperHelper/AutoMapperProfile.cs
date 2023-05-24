using AutoMapper;
using System.Reflection;
using OptiOverflow.Core.Interfaces.Common;

namespace OptiOverflow.Core.MapperHelper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.FullName!.StartsWith(nameof(OptiOverflow))).ToArray();
        ApplyMappingsFromAssembly(assemblies);
    }

    private void ApplyMappingsFromAssembly(IEnumerable<Assembly> assemblies)
    {
        var types = new List<Type>();
        foreach (var assembly in assemblies)
        {
            types.AddRange(assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces()
                    .Any(ct => ct.IsGenericType && ct.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList());
        }

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod("Mapping")
                             ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping");
            methodInfo?.Invoke(instance, new object?[] { this });
        }
    }
}