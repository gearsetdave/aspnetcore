// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Components.HtmlRendering;

namespace Microsoft.AspNetCore.Components.Web;

/// <summary>
/// Represents the output of rendering a component as HTML. The content can change if the component instance re-renders.
/// </summary>
public sealed class HtmlContent
{
    private readonly HtmlRendererCore _renderer;
    private readonly int _componentId;
    private readonly Task _quiescenceTask;

    internal HtmlContent(HtmlRendererCore renderer, int componentId, Task quiescenceTask)
    {
        _renderer = renderer;
        _componentId = componentId;
        _quiescenceTask = quiescenceTask;
    }

    /// <summary>
    /// Obtains a <see cref="Task"/> that completes when the component hierarchy has completed asynchronous tasks such as loading.
    /// </summary>
    /// <returns>A <see cref="Task"/> that completes when the component hierarchy has completed asynchronous tasks such as loading.</returns>
    public Task WaitForQuiescenceAsync()
        => _quiescenceTask;

    /// <summary>
    /// Returns an HTML string representation of the component's latest output.
    /// </summary>
    /// <returns>A task that completes with the HTML string.</returns>
    public async Task<string> ToHtmlStringAsync()
    {
        using var writer = new StringWriter();
        await WriteToAsync(writer);
        return writer.ToString();
    }

    /// <summary>
    /// Writes the component's latest output as HTML to the specified writer.
    /// </summary>
    /// <param name="output">The output destination.</param>
    /// <returns>A task representing the completion of the operation.</returns>
    public Task WriteToAsync(TextWriter output) => _renderer.Dispatcher.InvokeAsync(() =>
    {
        // The HTML-stringification process itself is synchronous, but WriteToAsync needs to be
        // async because we have to dispatch to the renderer's sync context.
        HtmlContentWriter.Write(_renderer, _componentId, output);
    });
}
