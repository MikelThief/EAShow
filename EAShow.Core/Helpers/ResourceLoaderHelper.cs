using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;

namespace EAShow.Core.Helpers
{
    public static class ResourceLoaderHelper
    {
        private static ResourceMap _resourceMap = null;

        public static ResourceMap ResourceMap
        {
            get
            {
                if (_resourceMap == null)
                {
                    _resourceMap = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap;
                }

                return _resourceMap;
            }
        }

        public static string GetString(string key)
        {
            return ResourceMap?.GetValue("Resources/" + key, new ResourceContext())?.ValueAsString;
        }

        public static string GetString(string key, ResourceContext context)
        {
            return ResourceMap?.GetValue("Resources/" + key, context)?.ValueAsString;
        }
    }
}
