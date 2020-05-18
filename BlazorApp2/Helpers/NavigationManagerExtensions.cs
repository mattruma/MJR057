using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace BlazorApp2.Helpers
{
    public static class NavigationManagerExtensions
    {
        public static T GetQueryStringValue<T>(
            this NavigationManager navigationManager,
            string key,
            T defaultValue)
        {
            var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var value))
            {
                if (typeof(T) == typeof(DateTime))
                {
                    return (T)(object)Convert.ToDateTime(value);
                }

                return (T)(object)value;
            }

            return defaultValue;
        }
    }
}