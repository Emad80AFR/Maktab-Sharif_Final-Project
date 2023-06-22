using Microsoft.Extensions.Logging;

namespace FrameWork.Infrastructure.ConfigurationModel;

public class AppSettingsOption
{


    public class Rootobject
    {
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public Domainsettings DomainSettings { get; set; }
    }

    public class Logging
    {
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
        public string MicrosoftAspNetCore { get; set; }
    }

    public class Domainsettings
    {
        public Codegenerator CodeGenerator { get; set; }
        public Payment Payment { get; set; }
    }

    public class Codegenerator
    {
        public string PreFixCode { get; set; }
    }

    public class Payment
    {
        public string method { get; set; }
        public string siteUrl { get; set; }
        public string merchant { get; set; }
    }

}








