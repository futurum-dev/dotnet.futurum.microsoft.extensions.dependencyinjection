namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

[RegisterAsSingleton.As<IService2>]
public class Automatic_Singleton_As_Service : IService1, IService2
{
}