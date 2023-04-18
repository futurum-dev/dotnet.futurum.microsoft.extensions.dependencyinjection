namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsSingleton.AsImplementedInterfacesAndSelf]
public class Automatic_Singleton_AsImplementedInterfacesAndSelf_Service : IService1, IService2
{
}