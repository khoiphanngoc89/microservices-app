using System.Reflection;

namespace Basket.Api.Infrastructure;

public class DependencyContextAssemblyBasketCustom
    : DependencyContextAssemblyCatalog
{
    public override IReadOnlyCollection<Assembly> GetAssemblies()
    {
        return [typeof(Program).Assembly];
    }
}
