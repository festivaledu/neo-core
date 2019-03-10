using System;

namespace Neo.Core.Communication.Packages
{
    public class EnterChannelPackageContent
    {
        public Guid ChannelId { get; set; }
        public string Password { get; set; }
    }
}
