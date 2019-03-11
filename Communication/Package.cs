using System;
using System.Security.Cryptography;
using Neo.Core.Authorization;
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
        ///     The <see cref="Package"/> contains <see cref="ServerMetaResponsePackageContent"/>.
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
        /// <summary>
        ///     The <see cref="Package"/> contains <see cref="InputPackageContent"/>.
        /// </summary>
        Input,
        /// <summary>
        ///     The <see cref="Package"/> contains <see cref="MessagePackageContent"/>.
        /// </summary>
        Message,
        /// <summary>
        ///     The <see cref="Package"/> contains nothing but informs the server that the <see cref="Client"/> has finished rendering.
        /// </summary>
        LoginFinished,
        /// <summary>
        ///     The <see cref="Package"/> contains a list of all <see cref="Channel"/>s.
        /// </summary>
        ChannelListUpdate,
        /// <summary>
        ///     The <see cref="Package"/> contains a list of all <see cref="Group"/>s.
        /// </summary>
        GroupListUpdate,
        /// <summary>
        ///     The <see cref="Package"/> contains a list of all <see cref="User"/>s.
        /// </summary>
        UserListUpdate,
        /// <summary>
        ///     The <see cref="Package"/> contains <see cref="EnterChannelPackageContent"/>.
        /// </summary>
        EnterChannel,
        /// <summary>
        ///     The <see cref="Package"/> contains <see cref="EnterChannelResponsePackageContent"/>.
        /// </summary>
        EnterChannelResponse,
        GetSettings,
        GetSettingsResponse,
        /// <summary>
        ///     The <see cref="Package"/> contains a <see cref="string"/> requesting a specific settings model.
        /// </summary>
        OpenSettings,
        /// <summary>
        ///     The <see cref="Package"/> contains <see cref="OpenSettingsResponsePackageContent"/>.
        /// </summary>
        OpenSettingsResponse,
        /// <summary>
        ///     The <see cref="Package"/> contains <see cref="EditSettingsPackageContent"/>.
        /// </summary>
        EditSettings,
        /// <summary>
        ///     The <see cref="Package"/> contains a <see cref="bool"/> determining whether the edit was successful or not.
        /// </summary>
        EditSettingsResponse,
        /// <summary>
        ///     The <see cref="Package"/> contains <see cref="EditProfilePackageContent"/>.
        /// </summary>
        EditProfile,
        /// <summary>
        ///     The <see cref="Package"/> contains <see cref="EditProfileResponsePackageContent"/>.
        /// </summary>
        EditProfileResponse,
        /// <summary>
        ///     The <see cref="Package"/> contains a dictionary of all known permissions and their human-readable names.
        /// </summary>
        KnownPermissionsUpdate,
        /// <summary>
        ///     The <see cref="Package"/> contains a <see cref="string"/> stating the reason for the following disconnect.
        /// </summary>
        DisconnectReason,
        /// <summary>
        ///     The <see cref="Package"/> contains <see cref="CreatePunishmentPackageContent"/>.
        /// </summary>
        CreatePunishment,
        /// <summary>
        ///     The <see cref="Package"/> contains a list of all <see cref="Account"/>s.
        /// </summary>
        AccountListUpdate,
        /// <summary>
        ///     The <see cref="Package"/> contains <see cref="CreateChannelPackageContent"/>.
        /// </summary>
        CreateChannel,
        /// <summary>
        ///     The <see cref="Package"/> contains <see cref="MessagePackageContent"/>.
        /// </summary>
        Mention,
        /// <summary>
        ///     The <see cref="Package"/> contains <see cref="CreateGroupPackageContent"/>.
        /// </summary>
        CreateGroup,
        /// <summary>
        ///     The <see cref="Package"/> contains a <see cref="bool"/> determining whether the creation was successful or not.
        /// </summary>
        CreateGroupResponse,
        /// <summary>
        ///     The <see cref="Package"/> contains a list of all the <see cref="Permission"/>s the user has been granted.
        /// </summary>
        GrantedPermissionsUpdate,
        /// <summary>
        ///     The <see cref="Package"/> contains a <see cref="Guid"/> of the <see cref="Group"/> to delete.
        /// </summary>
        DeleteGroup,
        /// <summary>
        ///     The <see cref="Package"/> contains a <see cref="bool"/> determining whether the deletion was successful or not.
        /// </summary>
        DeleteGroupResponse,
        /// <summary>
        ///     The <see cref="Package"/> contains a <see cref="Guid"/> of the <see cref="Channel"/> to delete.
        /// </summary>
        DeleteChannel,
        /// <summary>
        ///     The <see cref="Package"/> contains a <see cref="bool"/> determining whether the deletion was successful or not.
        /// </summary>
        DeleteChannelResponse,
        /// <summary>
        ///     The <see cref="Package"/> contains a <see cref="Guid"/> of the <see cref="Account"/> to unban.
        /// </summary>
        DeletePunishment,
        /// <summary>
        ///     The <see cref="Package"/> contains <see cref="AvatarPackageContent"/>.
        /// </summary>
        SetAvatar,
    }
}
