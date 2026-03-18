# RimBridgeServer.Annotations

RimBridgeServer.Annotations is a small shared annotation package for `RimBridgeServer`.

It exists for one job only: let third-party RimWorld mods describe tools with the same attribute model that RimBridgeServer uses, without taking a dependency on the full server runtime.

## What Is In This Package

The NuGet package, assembly, and default namespace are `RimBridgeServer.Annotations`.

It currently ships these attributes:

- `ToolAttribute`
- `ToolParameterAttribute`
- `ToolResponseAttribute`

Those types are pure metadata. They do not perform discovery, registration, transport, or invocation by themselves.

That split is intentional:

- mods depend on a tiny stable package
- `RimBridgeServer` stays responsible for scanning loaded mod assemblies and turning annotated methods into live bridge capabilities
- the shared surface remains cheap to version and cheap to adopt

## Example

```csharp
using RimBridgeServer.Annotations;

public sealed class AchtungBridgeTools
{
    [Tool("achtung/select_group", Description = "Select an Achtung pawn group by stable id.")]
    [ToolResponse("success", Type = "boolean", Description = "True when the group was resolved and selected.")]
    public object SelectGroup(
        [ToolParameter(Description = "Stable Achtung group id.")] string groupId)
    {
        return new
        {
            success = true,
            groupId
        };
    }
}
```

## Build

Run:

```bash
dotnet build RimBridgeServer.Annotations.sln
```

The library project is configured with `GeneratePackageOnBuild=true`, so a normal build emits:

- `artifacts/nuget/RimBridgeServer.Annotations.<version>.nupkg`
- `artifacts/nuget/RimBridgeServer.Annotations.<version>.snupkg`

That is the package you can upload to NuGet manually.

## Scope

This repository intentionally does not include:

- runtime assembly scanning
- bridge capability registration
- RimWorld integration code
- GABP transport code

Those belong in `RimBridgeServer`, not in the shared annotations package.
