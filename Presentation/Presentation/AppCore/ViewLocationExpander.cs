using System;
using Microsoft.AspNetCore.Mvc.Razor;

namespace BuildTest.AppCore;

public class ViewLocationExpander : IViewLocationExpander
{
    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        return JAppConfigStore.CustomSharedDirectories().Union(viewLocations);
    }

    public void PopulateValues(ViewLocationExpanderContext context)
    {
        context.Values["customviewlocation"] = nameof(ViewLocationExpander);
    }
}


