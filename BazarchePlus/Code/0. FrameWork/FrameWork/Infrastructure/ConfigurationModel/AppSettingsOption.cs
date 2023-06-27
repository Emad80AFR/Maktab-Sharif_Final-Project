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
        public Medals Medals { get; set; }
        public Saleamount SaleAmount { get; set; }
        public Wage Wage { get; set; }
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

    public class Medals
    {
        public int Gold { get; set; }
        public int Silver { get; set; }
        public int Bronze { get; set; }
    }

    public class Saleamount
    {
        public int GoldSale { get; set; }
        public int SilverSale { get; set; }
        public int BronzeSale { get; set; }
    }

    public class Wage
    {
        public int GoldenWage { get; set; }
        public int SilverWage { get; set; }
        public int BronzeWage { get; set; }
        public int Default { get; set; }
    }


}








