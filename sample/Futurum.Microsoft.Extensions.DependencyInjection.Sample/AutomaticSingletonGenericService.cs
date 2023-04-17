namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsSingleton<IService2>]
public class AutomaticSingletonGenericService : IService1, IService2
{
}