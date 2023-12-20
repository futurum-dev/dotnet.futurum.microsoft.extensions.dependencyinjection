namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsSingleton.AsKeyedImplementedInterfacesAndSelf("ServiceKey")]
public class Automatic_Keyed_Singleton_AsKeyedImplementedInterfacesAndSelf_Service : IService1, IService2
{
}