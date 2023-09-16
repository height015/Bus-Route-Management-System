using System;
using Presentation.Helpers;
using Microsoft.AspNetCore.Mvc.Razor;

public class JAppConfigStore
{
    private static string[] _customSharedDirectory = new[] { "Components", "LayoutTol", "Toolkit", "Widgets", "ComShared", "MainLayouts", "SideBars","NavTop" };

    public static List<string> CustomSharedDirectories()
    {
        List<string> locations = new();
        try
        {
            locations.Add($"/Views/Shared/{{0}}{RazorViewEngine.ViewExtension}");
            if (_customSharedDirectory != null && _customSharedDirectory.Any())
            {
                _customSharedDirectory.ForEachx(m =>
                {
                    locations.Add($"/Views/Shared/{m.Trim()}/{{0}}{RazorViewEngine.ViewExtension}");
                });
            }
            return locations;
        }
        catch (Exception ex)
        {
            //TODO Implement Error Logging 
            throw new CustomException(ex.StackTrace!, ex.Source!, ex.Message);
            
        }
    }
}
