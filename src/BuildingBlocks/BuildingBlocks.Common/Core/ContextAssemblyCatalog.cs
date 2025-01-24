using Carter;
using System.Reflection;

namespace BuildingBlocks.Common.Core;

public class ContextAssemblyCatalog
    : DependencyContextAssemblyCatalog
{
    private readonly Assembly[] _assemblies;
    public ContextAssemblyCatalog(Assembly assembly)
    {
        _assemblies = new[] { assembly };
    }

    public override IReadOnlyCollection<Assembly> GetAssemblies()
    {
        return _assemblies;
    }
}
