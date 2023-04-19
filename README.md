# Futurum.Microsoft.Extensions.DependencyInjection

![license](https://img.shields.io/github/license/futurum-dev/dotnet.futurum.microsoft.extensions.dependencyinjection?style=for-the-badge)
![CI](https://img.shields.io/github/actions/workflow/status/futurum-dev/dotnet.futurum.microsoft.extensions.dependencyinjection/ci.yml?branch=main&style=for-the-badge)
[![Coverage Status](https://img.shields.io/coveralls/github/futurum-dev/dotnet.futurum.microsoft.extensions.dependencyinjection?style=for-the-badge)](https://coveralls.io/github/futurum-dev/dotnet.futurum.microsoft.extensions.dependencyinjection?branch=main)
[![NuGet version](https://img.shields.io/nuget/v/futurum.microsoft.extensions.dependencyinjection?style=for-the-badge)](https://www.nuget.org/packages/futurum.microsoft.extensions.dependencyinjection)

A dotnet library that extends Microsoft.Extensions.DependencyInjection by adding support for [modules](#modules), [startables](#startables) and [attribute based registration](#attribute-based-registration).

- [x] Autodiscovery of DependencyInjection registrations, based on [attributes](#attribute-based-registration) and Source Generators
- [x] Autodiscovery of DependencyInjection modules, based on [attributes](#attribute-based-module) and Source Generators
- [x] Autodiscovery of DependencyInjection startables, based on [attributes](#attribute-based-startable) and Source Generators
- [x] [Roslyn Analysers](#roslyn-analysers) to help build your modules, startables and registrations, using best practices
- [x] Integration with [Futurum.Core](https://github.com/futurum-dev/dotnet.futurum.core)

## Modules
A module allows you to break up registration into logical units.

Module can either be registered using *IModule* interface and *AddModule* extension method, or by using the [*RegisterAsDependencyInjectionModule*](#attribute-based-module) attribute.

### IModule interface and AddModule extension method
#### IModule interface
Implements this interface to create a module.

```csharp
public class TestModule : IModule
{
    public void Load(IServiceCollection services)
    {
        services.AddSingleton<ITestService, TestService>();
    }
}
```

#### AddModule extension method
Allows you to register a module.

```csharp
services.AddModule<TestModule>();
```

```csharp
services.AddModule(new TestModule());
```

### Attribute based module
You can also register modules using attributes.

- RegisterAsDependencyInjectionModule attribute

```csharp
public class Module
{
    [RegisterAsDependencyInjectionModule]
    public void Load(IServiceCollection services)
    {
    }
}
```

```csharp
public static class Module
{
    [RegisterAsDependencyInjectionModule]
    public static void Load(IServiceCollection services)
    {
    }
}
```

## Startables
A startable is resolved at the start of the application lifecycle and is a place to perform actions as soon as the DependencyInjection container is built.

Startable can either be registered using *IStartable* interface and *AddStartable* extension method, or by using the [*RegisterAsDependencyInjectionStartable*](#attribute-based-startable) attribute.

### IStartable interface and AddStartable extension method
#### IStartable interface
Implements this interface to create a startable.

```csharp
public class TestStartable : IStartable
{
    public Task Start()
    {
        // Do something
    }
}
```

#### AddStartable extension method
Allows you to register a startable.

```csharp
services.AddStartable<TestStartable>();
```

```csharp
services.AddStartable(new TestStartable());
```

### Attribute based startable
You can also register modules using attributes.

- RegisterAsDependencyInjectionStartable attribute

```csharp
public class Startable
{
    [RegisterAsDependencyInjectionStartable]
    public Task Start()
    {
        // Do something
    }
}
```

```csharp
public static class Startable
{
    [RegisterAsDependencyInjectionStartable]
    public static Task Start()
    {
        // Do something
    }
}
```

### BuildServiceProviderWithStartables extension method
If you are manually building the *IServiceProvider*, then you need to use *BuildServiceProviderWithStartablesAsync* extension method.
This will build the container as usual, but also starts all *IStartable* instances.

```csharp
var serviceProvider = await services.BuildServiceProviderWithStartablesAsync();
```

## Attribute based registration
You can also register services using attributes.

The attributes have been created is a discoverable way. They take the following form:
- RegisterAs{Lifetime}
- RegisterAs{Lifetime}.AsSelf
- RegisterAs{Lifetime}.As{ServiceType}
- RegisterAs{Lifetime}.AsImplementedInterfaces
- RegisterAs{Lifetime}.AsImplementedInterfacesAndSelf
- RegisterAs{Lifetime}.AsOpenGeneric

There are 3 lifetimes available:
- Singleton
- Scoped
- Transient

### RegisterAs{Lifetime} attribute
This will register the class against the 1 interface the class implements, for the specified lifetime.

e.g. This will register *Service* against *IService* with a *Scoped* lifetime.
```csharp
[RegisterAsScoped]
public class Service : IService
{
}
```

### RegisterAs{Lifetime}.AsSelf attribute
This will register the class against itself, for the specified lifetime.

e.g. This will register *Service* against *Service* with a *Scoped* lifetime.
```csharp
[RegisterAsScoped.AsSelf]
public class Service
{
}
```

### RegisterAs{Lifetime}.As{ServiceType} attribute
This will register the class against the specified interface, for the specified lifetime.

e.g. This will register *Service* against *IService1* with a *Scoped* lifetime.
```csharp
[RegisterAsScoped.As<IService2>]
public class Service : IService1, IService2
{
}
```

### RegisterAs{Lifetime}.AsImplementedInterfaces attribute
This will register the class against all the interfaces it implements directly, for the specified lifetime.

e.g. This will register *Service* against *IService1* and *IService2* with a *Scoped* lifetime.
```csharp
[RegisterAsScoped.AsImplementedInterfaces]
public class Service : IService1, IService2
{
}
```

### RegisterAs{Lifetime}.AsImplementedInterfacesAndSelf attribute
This will register the class against all the interfaces it implements directly and itself, for the specified lifetime.

e.g. This will register *Service* against *Service*, *IService1* and *IService2* with a *Scoped* lifetime.
```csharp
[RegisterAsScoped.AsImplementedInterfacesAndSelf]
public class Service : IService1, IService2
{
}
```

### RegisterAs{Lifetime}.AsOpenGeneric attribute
This will register the an open generic class against an open generic interface, for the specified lifetime.

e.g. This will register *Service&lt;T&gt;* against *IService&lt;T&gt;* with a *Scoped* lifetime.
```csharp
[RegisterAsScoped.AsOpenGeneric(ImplementationType = typeof(Service<>), ServiceType = typeof(IService<>))]
public class Service<T> : IService<T>
{
}
```

### DuplicateRegistrationStrategy
You can also specify how to handle duplicate registrations.

- Try - Adds the new registration, if the service hasn't already been registered
- Replace - Removes any existing registration and then adds the new registration
- Add - Adds the new registration, irrespective of if its previously been registered

**NOTE** - This defaults to *Try*

```csharp
[RegisterAsSingleton(DuplicateRegistrationStrategy = DuplicateRegistrationStrategy.Add)]
public class Service : IService
{
}
```

## TryGetService
Try to get the service object of the specified type.

```csharp
var result = serviceProvider.TryGetService<ITestService>();
```

## Roslyn Analysers
- FMEDI0001 - Invalid Module Parameter
- FMEDI0002 - Missing Module Parameter
- FMEDI0003 - Non empty constructor found on Module
- FMEDI0004 - Non empty constructor found on Startable
- FMEDI0005 - Non async method found on Startable
- FMEDI0006 - Non void method found on Module
- FMEDI0007 - Register ServiceType not implemented by class
- FMEDI0008 - Registration, must only have 1 interface
- FMEDI1000 - Registration information