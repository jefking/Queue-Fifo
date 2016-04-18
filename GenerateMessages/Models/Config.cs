namespace Models
{
    using System;
    using System.Configuration;

    public class Config
    {
        public readonly Guid[] Devices = new Guid[] { Guid.Parse("{5C82AC08-6AF1-4111-8848-AB53F3B3B53C}"), Guid.Parse("{23646E25-2DD7-4770-8246-BDFDF6013FCC}"), Guid.Parse("{FCCCC599-B381-4926-A479-BB11F4687B26}"), Guid.Parse("{D6F1558E-D091-4150-B3EF-8E4A678063FB}") };
        public readonly string Connection = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
    }
}