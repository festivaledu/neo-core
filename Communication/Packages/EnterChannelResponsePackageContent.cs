using Neo.Core.Management;
using Neo.Core.Shared;

namespace Neo.Core.Communication.Packages
{
    public class EnterChannelResponsePackageContent
    {
        public ChannelActionResult Result { get; set; }
        public Channel Channel { get; set; }

        public EnterChannelResponsePackageContent(ChannelActionResult result, Channel channel = null) {
            this.Result = result;
            this.Channel = channel;
        }
    }
}
