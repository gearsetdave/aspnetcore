// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Components.Rendering;

namespace Microsoft.AspNetCore.Components;

internal interface ICascadingValueSupplier
{
    // This interface exists only so that CascadingParameterState has a way
    // to work with all CascadingValue<T> types regardless of T.

    bool CanSupplyValue(in CascadingParameterInfo parameterInfo);

    object? GetCurrentValue(in CascadingParameterInfo parameterInfo);

    bool CurrentValueIsFixed { get; }

    void Subscribe(ComponentState subscriber);

    void Unsubscribe(ComponentState subscriber);
}