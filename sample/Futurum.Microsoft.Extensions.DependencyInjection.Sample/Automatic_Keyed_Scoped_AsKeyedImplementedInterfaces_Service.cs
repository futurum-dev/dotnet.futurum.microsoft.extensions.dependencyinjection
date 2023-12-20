namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsScoped.AsKeyedImplementedInterfaces("ServiceKey")]
public class Automatic_Keyed_Scoped_AsKeyedImplementedInterfaces_Service : IService1, IService2
{
}