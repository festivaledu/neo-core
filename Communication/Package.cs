using System.Security.Cryptography;
using Neo.Core.Communication.Packages;
using Neo.Core.Cryptography;
using Neo.Core.Networking;
using Neo.Core.Shared;
using Newtonsoft.Json;

namespace Neo.Core.Communication
{
    /// <summary>
    ///     Encapsulates any form of content. Used for all communication between server and client.
    /// </summary>
    public class Package
    {
        /// <summary>
        ///     The content of this <see cref="Package"/>.
        /// </summary>
        public dynamic Content { get; set; }

        /// <summary>
        ///     The type of content this <see cref="Package"/> includes.
        /// </summary>
        public PackageType Type { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Package"/> class.
        /// </summary>
        public Package() { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Package"/> class with a type and any content.
        /// </summary>
        /// <param name="type">The type to set.</param>
        /// <param name="content">The content to set.</param>
        public Package(PackageType type, dynamic content) {
            this.Type = type;
            this.Content = content;
        }

        /// <summary>
        ///     Returns the content of this <see cref="Package"/> typesafe.
        /// </summary>
        /// <typeparam name="TOut">The type to cast <see cref="Content"/> to.</typeparam>
        /// <returns>Returns <see cref="Content"/> casted to <see cref="TOut"/>.</returns>
        public TOut GetContentTypesafe<TOut>() {
            return JsonConvert.DeserializeObject<TOut>(JsonConvert.SerializeObject(Content));
        }
    }

    /// <summary>
    ///     Specifies the type of content a <see cref="Package"/> contains.
    /// </summary>
    public enum PackageType
    {
        /// <summary>
        ///     The <see cref="Package"/> contains debugging information.
        /// </summary>
        Debug,
        /// <summary>
        ///     The <see cref="Package"/> contains public <see cref="RSAParameters"/>.
        /// </summary>
        RSA,
        /// <summary>
        ///     The <see cref="Package"/> contains <see cref="AesParameters"/>.
        /// </summary>
        AES,
        /// <summary>
        ///     The <see cref="Package"/> contains nothing but requests a <see cref="MetaResponse"/>.
        /// </summary>
        Meta,
        /// <summary>
        ///     The <see cref="Package"/> contains <see cref="ServerMetaPackageContent"/>.
        /// </summary>
        MetaResponse,
        /// <summary>
        ///     The <see cref="Package"/> contains <see cref="RegisterPackageContent"/>.
        /// </summary>
        Register,
        /// <summary>
        ///     The <see cref="Package"/> contains <see cref="MemberLoginPackageContent"/>.
        /// </summary>
        MemberLogin,
        /// <summary>
        ///     The <see cref="Package"/> contains an <see cref="Identity"/>.
        /// </summary>
        GuestLogin,
        /// <summary>
        ///     The <see cref="Package"/> contains a status message and an <see cref="Identity"/> if the login was successful.
        /// </summary>
        LoginResponse,
        Input,
        Message,
        /// <summary>
        ///     The <see cref="Package"/> contains nothing but informs the server that the <see cref="Client"/> has finished rendering.
        /// </summary>
        LoginFinished,
        ChannelListUpdate,
        GroupListUpdate,
        UserListUpdate,
        EnterChannel,
        EnterChannelResponse,
        GetSettings,
        GetSettingsResponse,
        OpenSettings,
        OpenSettingsResponse,
        EditSettings,
        EditSettingsResponse,
        EditProfile,
        EditProfileResponse,
        KnownPermissionsUpdate,
        DisconnectReason,
        CreatePunishment,
    }
}
