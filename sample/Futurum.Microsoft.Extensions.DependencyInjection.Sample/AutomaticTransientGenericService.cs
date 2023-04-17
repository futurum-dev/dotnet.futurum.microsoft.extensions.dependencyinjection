namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsTransient<IService2>]
public class AutomaticTransientGenericService : IService1, IService2
{
}