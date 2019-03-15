using Neo.Core.Management;
using Neo.Core.Shared;

namespace Neo.Core.Communication.Packages
{
    /// <summary>
    ///     Contains the data for the <see cref="PackageType.EnterChannelResponse"/> package.
    /// </summary>
    public class EnterChannelResponsePackageContent
    {
        /// <summary>
        ///     The result of the action.
        /// </summary>
        public ChannelActionResult Result { get; set; }

        /// <summary>
        ///     The channel that has been entered.
        /// </summary>
        public Channel Channel { get; set; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="EnterChannelResponsePackageContent"/> class.
        /// </summary>
        /// <param name="result">The result of the action.</param>
        /// <param name="channel">The channel that has been entered.</param>
        public EnterChannelResponsePackageContent(ChannelActionResult result, Channel channel = null) {
            this.Result = result;
            this.Channel = channel;
        }
    }
}
