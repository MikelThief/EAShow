using System;

namespace EAShow.Core
{
    // TODO: Place secrets in-app at compile-time. Currently secrets are taken from environment variables.
    public static class Secrets
    {
        public static string SYNCFUSION_SECRET = Environment.GetEnvironmentVariable(variable: "SYNCFUSION_SECRET");
    }
}
