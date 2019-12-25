using System.IO;
using Windows.Storage;

namespace EAShow.Core.Helpers
{
    public static class LiteDbConnectionStringHelper
    {
        public static string GetRoamingDbConnectionString()
        {
            return Path.Combine(path1: ApplicationData.Current.RoamingFolder.Path,
                path2: "RoamingDatabase.db");
        }
    }
}
