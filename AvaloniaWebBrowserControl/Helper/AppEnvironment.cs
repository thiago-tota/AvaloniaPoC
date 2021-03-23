using AvaloniaWebBrowserControl.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaWebBrowserControl.Helper
{
    public static partial class AppEnvironment
    {
        public static IConfigurationRoot? Configuration { get; private set; }

        public static class EmbeddedBrowser
        {
            private static List<EmbeddedBrowserSettings>? settings;

            public static List<EmbeddedBrowserSettings> Settings
            {
                get
                {
                    if (settings == null)
                    {
                        settings = new List<EmbeddedBrowserSettings>();

                        foreach (var item in Configuration.GetSection($"AppSettings:EmbeddedBrowserSettings").GetChildren().Select(f => f.Path))
                        {
                            settings.Add(new EmbeddedBrowserSettings
                            {
                                Key = Configuration.GetSection(item).GetSection("Key").Value,
                                Value = Configuration.GetSection(item).GetSection("Value").Value,
                                Action = (EmbeddedBrowserSettingsAction)Enum.Parse(typeof(EmbeddedBrowserSettingsAction), Configuration.GetSection(item).GetSection("Action").Value, true)
                            });
                        }
                    }

                    return settings;
                }
            }
        }
    }
}
