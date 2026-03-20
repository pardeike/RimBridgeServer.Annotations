using System;

namespace RimBridgeServer.Annotations;

/// <summary>
/// Marks a method as a bridge tool entrypoint.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class ToolAttribute : Attribute
{
    /// <summary>
    /// Initializes a new tool annotation with the public tool name.
    /// </summary>
    /// <param name="name">Stable tool name such as <c>rimworld/list_colonists</c>.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null, empty, or whitespace.</exception>
    public ToolAttribute(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tool name cannot be null or empty.", nameof(name));

        Name = name;
    }

    /// <summary>
    /// Stable externally visible tool name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Optional human-readable description of the tool.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Optional human-readable title for display surfaces.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Optional human-readable summary of what a successful result means or what useful handle it returns.
    /// </summary>
    public string? ResultDescription { get; set; }

    /// <summary>
    /// Indicates whether the tool requires authentication when exposed over a transport that supports auth.
    /// </summary>
    public bool RequiresAuth { get; set; } = true;
}

/// <summary>
/// Documents one method parameter for a bridge tool.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class ToolParameterAttribute : Attribute
{
    /// <summary>
    /// Optional human-readable parameter description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indicates whether the parameter is required.
    /// </summary>
    public bool Required { get; set; } = true;

    /// <summary>
    /// Optional fallback value to advertise in metadata.
    /// </summary>
    public object? DefaultValue { get; set; }
}

/// <summary>
/// Documents one response field for a bridge tool.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class ToolResponseAttribute : Attribute
{
    /// <summary>
    /// Initializes a new response-field annotation.
    /// </summary>
    /// <param name="name">Stable JSON field name in the response payload.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null, empty, or whitespace.</exception>
    public ToolResponseAttribute(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Response field name cannot be null or empty.", nameof(name));

        Name = name;
    }

    /// <summary>
    /// Stable response field name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// JSON schema-style field type description.
    /// </summary>
    public string Type { get; set; } = "string";

    /// <summary>
    /// Optional human-readable field description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indicates whether the field is always present in successful responses.
    /// </summary>
    public bool Always { get; set; } = true;

    /// <summary>
    /// Indicates whether the field may be null.
    /// </summary>
    public bool Nullable { get; set; }
}
