namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsTransient.AsKeyedImplementedInterfacesAndSelf("ServiceKey")]
public class Automatic_Keyed_Transient_AsKeyedImplementedInterfacesAndSelf_Service : IService1, IService2
{
}