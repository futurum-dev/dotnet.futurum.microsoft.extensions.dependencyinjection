# Futurum.Microsoft.Extensions.DependencyInjection

![license](https://img.shields.io/github/license/futurum-dev/dotnet.futurum.microsoft.extensions.dependencyinjection?style=for-the-badge)
![CI](https://img.shields.io/github/actions/workflow/status/futurum-dev/dotnet.futurum.microsoft.extensions.dependencyinjection/ci.yml?branch=main&style=for-the-badge)
[![Coverage Status](https://img.shields.io/coveralls/github/futurum-dev/dotnet.futurum.microsoft.extensions.dependencyinjection?style=for-the-badge)](https://coveralls.io/github/futurum-dev/dotnet.futurum.microsoft.extensions.dependencyinjection?branch=main)
[![NuGet version](https://img.shields.io/nuget/v/futurum.microsoft.extensions.dependencyinjection?style=for-the-badge)](https://www.nuget.org/packages/futurum.microsoft.extensions.dependencyinjection)

A dotnet library, that allows Microsoft.Extensions.DependencyInjection to work with Futurum.Core. It also adds support for modules and startables.

- [x] Autodiscovery of DependencyInjection registrations, based on [attributes](#attribute-based-registration) and Source Generators
- [x] Autodiscovery of DependencyInjection modules, based on [attributes](#attribute-based-module) and Source Generators
- [x] Autodiscovery of DependencyInjection startables, based on [attributes](#attribute-based-startable) and Source Generators
- [x] [Roslyn Analysers](#roslyn-analysers) to help build your WebApiEndpoint(s), using best practices

## TryGetService
Try to get the service object of the specified type.

```csharp
var result = serviceProvider.TryGetService<ITestService>();
```

## Modules
A module allows you to break up registration into logical units.

### IModule interface
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

### AddModule extension method
Allows you to register a module.

```csharp
services.AddModule<TestModule>();
```

```csharp
services.AddModule(new TestModule());
```

## Startables
A startable is resolved at the start of the application lifecycle and is a place to perform actions as soon as the DependencyInjection container is built.

### IStartable interface
Implements this interface to create a startable.

```csharp
public class TestStartable : IStartable
{
    public void Start()
    {
        // Do something
    }
}
```

### AddStartable extension method
Allows you to register a startable.

```csharp
services.AddStartable<TestStartable>();
```

```csharp
services.AddStartable(new TestStartable());
```

### BuildServiceProviderWithStartables extension method
Creates a ServiceProvider containing services from the provided IServiceCollection and starts all *IStartable* instances.

```csharp
var serviceProvider = services.BuildServiceProviderWithStartables();
```

## Attribute based registration
You can also register services using attributes.

- RegisterAsSingleton attribute
- RegisterAsScoped attribute
- RegisterAsTransient attribute

### DuplicateRegistrationStrategy
- Try - Adds the new registration, if the service hasn't already been registered
- Replace - Removes any existing registration and then adds the new registration
- Add - Adds the new registration, irrespective of if its previously been registered

**NOTE** - This defaults to *Try*

### InterfaceRegistrationStrategy
- Self - Registers the service as itself
- ImplementedInterfaces - Registers the service as each its implemented interfaces
- SelfWithInterfaces - Registers the service as itself and each its implemented interfaces

**NOTE** - This defaults to *SelfWithInterfaces*

## Attribute based module
You can also register modules using attributes.

- RegisterAsDependencyInjectionModule attribute

## Attribute based startable
You can also register modules using attributes.

- RegisterAsDependencyInjectionStartable attribute

## Roslyn Analysers
- FMEDI0001 - Invalid Module Parameter
- FMEDI0002 - Missing Module Parameter
- FMEDI0003 - Non empty constructor found on Module
- FMEDI0004 - Non empty constructor found on Startable