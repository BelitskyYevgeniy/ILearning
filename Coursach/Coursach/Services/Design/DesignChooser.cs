using Coursach.Interfaces;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Coursach.Services.Design
{
    public class DesignChooser : IDesignChooser
    {
        
        private Dictionary<string, string> resources;
        public string Standart
        {
            get;
        }
        public string Current { get;set;}
        public string TakeTheme { 
            get
            {
                return resources[Current];
            }
        }
        public SelectList Themes
        {
            get
            {
                return new SelectList(resources.Keys);
            }
        }

        public DesignChooser()
        {
            /*Standart = standart;
            resources = res;*/
            resources = new Dictionary<string, string>()
            {
                {"light", "/css/themes/light.css" },
                {"dark", "/css/themes/dark.css" }
            };
            Current =  "light";
            Standart = resources["light"];
          

        }
    }
    
 
    public static class ServiceProviderExtensions
    {
        public static void AddDesignChooserService(this IServiceCollection services)
        {
            services.AddScoped<IDesignChooser, DesignChooser>();
        }
    }
}