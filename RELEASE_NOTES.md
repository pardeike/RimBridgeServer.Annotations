# Release Notes

## 1.1.0

This release is aimed at engineers building RimBridgeServer-style bridge hosts and at mod or plugin authors whose tools those hosts can discover.

### Highlights

- Adds optional `Tool.ResultDescription` to the shared annotation contract
- Keeps the package metadata-only and lightweight for third-party extensions
- Clarifies the repository's role as the shared contract between a bridge host and discoverable external mods

### Compatibility

- Backward-compatible with `RimBridgeServer.Annotations 1.0.0`
- Existing mods using the 1.0.0 attributes do not need source changes
- Current host-side consumers can adopt `ResultDescription` incrementally
- Ships `net472`, `netstandard2.0`, and `net10.0` assets

### Why This Package Exists

`Lib.GAB` is the host/runtime layer for products that expose a final GABP surface.

`RimBridgeServer.Annotations` is the smaller package that can be shared with external mods or plugins so they can describe tools without depending on the full bridge/runtime implementation.

That split lets a RimBridgeServer-style product:

- keep bridge/runtime dependencies inside the host
- scan external assemblies for contributed tools
- republish those tools through one bridge surface

### Upgrade Notes

- Bridge hosts that already surface richer tool metadata can start reading `Tool.ResultDescription`
- Third-party mods can add `ResultDescription` gradually; it remains optional
- Field-level `[ToolResponse]` metadata remains optional and independent
