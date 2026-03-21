# RimBridgeServer.Annotations

RimBridgeServer.Annotations is a small shared annotation package for products that work like `RimBridgeServer`.

## Overview

`RimBridgeServer.Annotations` is not a bridge runtime, not a transport implementation, and not an end-user server product. It is the lightweight annotation contract shared between a bridge host and the external mods or plugins whose tools that host can discover.

For the `1.1.0` release summary and upgrade notes, see [`RELEASE_NOTES.md`](RELEASE_NOTES.md).

This repository exists to keep that shared contract separate from the full bridge/runtime layer:

- a RimBridgeServer-style host can use `Lib.GAB` plus its own product logic to expose the final GABP surface
- the host itself can use this same attribute model for discoverable tools
- third-party mods can reference only `RimBridgeServer.Annotations` to describe tools without taking a dependency on the full bridge runtime
- the host can scan loaded assemblies and republish discovered tools through one bridge surface

If you are building your own version of RimBridgeServer for another game, this is the reason this repo exists: keep the host/runtime dependency in the bridge, and keep the external extension contract small, stable, and metadata-only.

## Who This Is For

Use this package if you are:

- building a bridge host that wants to discover attributed tools from extension assemblies
- writing a mod or plugin that should contribute tools to such a host without taking a dependency on the full bridge runtime

## What Is In This Package

The NuGet package, assembly, and default namespace are `RimBridgeServer.Annotations`.

It currently ships these attributes:

- `ToolAttribute`
- `ToolParameterAttribute`
- `ToolResponseAttribute`

Those types are pure metadata. They do not perform discovery, registration, transport, or invocation by themselves.

That split is intentional:

- bridge hosts and extension mods share one stable annotation model
- contributed extensions depend on a tiny package instead of the full bridge/runtime layer
- `RimBridgeServer` or your own bridge host stays responsible for scanning loaded assemblies and turning annotated methods into live bridge capabilities
- the shared surface remains cheap to version and cheap to adopt

## How This Fits With Lib.GAB

`Lib.GAB` is the host/runtime layer for teams building a RimBridgeServer-style bridge product.

`RimBridgeServer.Annotations` is the lighter package that can be shared with external mods and plugin assemblies that contribute tools to that host.

That means the responsibilities stay split cleanly:

- `Lib.GAB` handles the host-side GABP server mechanics
- `RimBridgeServer.Annotations` supplies the shared metadata contract
- `RimBridgeServer` or your own product owns discovery, trust boundaries, naming rules, and lifecycle management for contributed tools

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

Those belong in `RimBridgeServer` or in your own bridge host product, not in the shared annotations package.
