namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsSingleton.AsKeyed<IService2>("ServiceKey")]
public class Automatic_Keyed_Singleton_AsKeyed_Service : IService1, IService2
{
}