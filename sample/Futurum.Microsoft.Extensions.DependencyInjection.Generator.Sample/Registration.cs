namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Sample;

public interface IService1
{
}

public interface IService2
{
}

public interface IService3
{
}

[RegisterAsTransient(InterfaceRegistrationStrategy = InterfaceRegistrationStrategy.Self)]
public class TransientService_Self : IService1
{
}

[RegisterAsTransient(InterfaceRegistrationStrategy = InterfaceRegistrationStrategy.ImplementedInterfaces)]
public class TransientService_ImplementedInterfaces : IService1
{
}

[RegisterAsTransient(InterfaceRegistrationStrategy = InterfaceRegistrationStrategy.SelfWithInterfaces)]
public class TransientService_SelfWithInterfaces : IService1
{
}

[RegisterAsScoped(InterfaceRegistrationStrategy = InterfaceRegistrationStrategy.Self)]
public class ScopedService_Self : IService2
{
}

[RegisterAsScoped(InterfaceRegistrationStrategy = InterfaceRegistrationStrategy.ImplementedInterfaces)]
public class ScopedService_ImplementedInterfaces : IService2
{
}

[RegisterAsScoped(InterfaceRegistrationStrategy = InterfaceRegistrationStrategy.SelfWithInterfaces)]
public class ScopedService_SelfWithInterfaces : IService2
{
}

[RegisterAsSingleton(InterfaceRegistrationStrategy = InterfaceRegistrationStrategy.Self)]
public class SingletonService_Self : IService3
{
}

[RegisterAsSingleton(InterfaceRegistrationStrategy = InterfaceRegistrationStrategy.ImplementedInterfaces)]
public class SingletonService_ImplementedInterfaces : IService3
{
}

[RegisterAsSingleton(InterfaceRegistrationStrategy = InterfaceRegistrationStrategy.SelfWithInterfaces)]
public class SingletonService_SelfWithInterfaces : IService3
{
}

[RegisterAsScoped(DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Try)]
public class SingletonService_Try : IService1
{
}

[RegisterAsScoped(DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Replace)]
public class SingletonService_Replace : IService1
{
}

[RegisterAsScoped(DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Add)]
public class SingletonService_Add : IService1
{
}

[RegisterAsScoped(ServiceType = typeof(IService2), DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Add)]
public class ServiceMultipleInterfaces : IService1, IService2
{
}

public interface IService_OpenGeneric<T>
{
}

[RegisterAsSingleton(ImplementationType = typeof(TransientServiceOpenGeneric<>), ServiceType = typeof(IService_OpenGeneric<>))]
public class TransientServiceOpenGeneric<T> : IService_OpenGeneric<T>
{
}

[RegisterAsSingleton(ImplementationType = typeof(ScopedServiceOpenGeneric<>), ServiceType = typeof(IService_OpenGeneric<>))]
public class ScopedServiceOpenGeneric<T> : IService_OpenGeneric<T>
{
}

[RegisterAsSingleton(ImplementationType = typeof(SingletonServiceOpenGeneric<>), ServiceType = typeof(IService_OpenGeneric<>))]
public class SingletonServiceOpenGeneric<T> : IService_OpenGeneric<T>
{
}