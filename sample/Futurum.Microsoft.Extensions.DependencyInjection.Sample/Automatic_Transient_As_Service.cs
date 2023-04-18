namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsTransient.As<IService2>]
public class Automatic_Transient_As_Service : IService1, IService2
{
}