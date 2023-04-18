namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsScoped.AsImplementedInterfacesAndSelf]
public class Automatic_Scoped_AsImplementedInterfacesAndSelf_Service : IService1, IService2
{
}