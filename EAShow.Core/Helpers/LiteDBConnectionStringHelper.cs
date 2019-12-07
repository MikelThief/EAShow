using System.IO;
using Windows.Storage;

namespace EAShow.Core.Helpers
{
    public static class LiteDbConnectionStringHelper
    {
        public static string GetConnectionString()
        {
            return Path.Combine(path1: ApplicationData.Current.LocalCacheFolder.Path,
                path2: "Database.db");
        }
    }
}
