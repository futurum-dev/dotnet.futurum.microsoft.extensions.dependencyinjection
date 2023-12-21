namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsScoped.AsKeyedImplementedInterfacesAndSelf("ServiceKey")]
public class Automatic_Keyed_Scoped_AsKeyedImplementedInterfacesAndSelf_Service : IService1, IService2
{
}