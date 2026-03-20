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
    [Tool(
        "achtung/select_group",
        Description = "Select an Achtung pawn group by stable id.",
        ResultDescription = "Whether the group was selected and which stable id was resolved.")]
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

Use `ResultDescription` when you want to describe what a successful result means without documenting every response field. Use `ToolResponseAttribute` when individual response fields are worth surfacing in machine-readable metadata.

## Attention And Async Problems

Extension tools do not currently need to publish GABP attention items directly.

Today, blocking attention is still owned centrally by RimBridgeServer:

- severe async RimWorld log errors can open attention
- failed or timed-out bridge operations can open attention
- ordinary extension tools usually do not need to write any attention protocol code

There is not yet a public cross-mod API for third-party mods to publish their own async attention items. If your extension needs that, treat it as future integration work rather than part of the current annotations contract.

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
