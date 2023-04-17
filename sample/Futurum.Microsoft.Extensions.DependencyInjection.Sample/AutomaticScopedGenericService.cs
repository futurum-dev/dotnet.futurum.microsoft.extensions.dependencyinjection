namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsScoped<IService2>]
public class AutomaticScopedGenericService : IService1, IService2
{
}