namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsSingleton.AsKeyedImplementedInterfaces("ServiceKey")]
public class Automatic_Keyed_Singleton_AsKeyedImplementedInterfaces_Service : IService1, IService2
{
}