namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsTransient.AsImplementedInterfacesAndSelf]
public class Automatic_Transient_AsImplementedInterfacesAndSelf_Service : IService1, IService2
{
}