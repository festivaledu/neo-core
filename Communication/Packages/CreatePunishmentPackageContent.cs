using System;

namespace Neo.Core.Communication.Packages
{
    public class CreatePunishmentPackageContent
    {
        public Guid Target { get; set; }
        public string Action { get; set; }
    }
}
