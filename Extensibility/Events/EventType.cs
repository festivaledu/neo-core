using System;
using Neo.Core.Communication;
using Neo.Core.Communication.Packages;
using Neo.Core.Networking;
using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Specifies the type of an event.
    /// </summary>
    public enum EventType
    {
        /// <summary>
        ///     This event is created and raised by a <see cref="Plugin"/>.
        /// </summary>
        Custom,

        /// <summary>
        ///     This event is raised after the <see cref="BaseServer"/> was initialized.
        /// </summary>
        ServerInitialized,

        /// <summary>
        ///     This event is raised after a <see cref="Client"/> connected to the server.
        /// </summary>
        Connected,
        /// <summary>
        ///     This event is raised after a <see cref="Client"/> disconnected from the server.
        /// </summary>
        Disconnected,
        /// <summary>
        ///     This event is raised after a <see cref="User"/> joined the server.
        /// </summary>
        ServerJoined,
        /// <summary>
        ///     This event is raised after a <see cref="User"/> finished the login.
        /// </summary>
        Login,
        /// <summary>
        ///     This event is raised after a <see cref="User"/> left the server.
        /// </summary>
        ServerLeft,

        /// <summary>
        ///     This event is raised before a new <see cref="Account"/> is created.
        /// </summary>
        BeforeAccountCreate,
        /// <summary>
        ///     This event is raised after a new <see cref="Account"/> was created.
        /// </summary>
        AccountCreated,
        /// <summary>
        ///     This event is raised before a new <see cref="Channel"/> is created.
        /// </summary>
        BeforeChannelCreate,
        /// <summary>
        ///     This event is raised after a new <see cref="Channel"/> was created.
        /// </summary>
        ChannelCreated,
        /// <summary>
        ///     This event is raised before a new <see cref="Group"/> is created.
        /// </summary>
        BeforeGroupCreate,
        /// <summary>
        ///     This event is raised after a new <see cref="Group"/> was created.
        /// </summary>
        GroupCreated,

        /// <summary>
        ///     This event is raised before an <see cref="Account"/> is edited.
        /// </summary>
        BeforeAccountEdit,
        /// <summary>
        ///     This event is raised after an <see cref="Account"/> was edited.
        /// </summary>
        AccountEdited,
        /// <summary>
        ///     This event is raised before a <see cref="Channel"/> is edited.
        /// </summary>
        BeforeChannelEdit,
        /// <summary>
        ///     This event is raised after a <see cref="Channel"/> was edited.
        /// </summary>
        ChannelEdited,
        /// <summary>
        ///     This event is raised before a <see cref="Group"/> is edited.
        /// </summary>
        BeforeGroupEdit,
        /// <summary>
        ///     This event is raised after a <see cref="Group"/> was edited.
        /// </summary>
        GroupEdited,
        /// <summary>
        ///     This event is raised before an <see cref="Identity"/> is edited.
        /// </summary>
        BeforeIdentityEdit,
        /// <summary>
        ///     This event is raised after an <see cref="Identity"/> was edited.
        /// </summary>
        IdentityEdited,

        /// <summary>
        ///     This event is raised before an <see cref="User"/> joins a <see cref="Channel"/>.
        /// </summary>
        BeforeChannelJoin,
        /// <summary>
        ///     This event is raised after an <see cref="User"/> joined a <see cref="Channel"/>.
        /// </summary>
        ChannelJoined,
        /// <summary>
        ///     This event is raised before a <see cref="Member"/> joins a <see cref="Group"/>.
        /// </summary>
        BeforeGroupJoin,
        /// <summary>
        ///     This event is raised after a <see cref="Member"/> joined a <see cref="Group"/>.
        /// </summary>
        GroupJoined,

        /// <summary>
        ///     This event is raised before an <see cref="User"/> leaves a <see cref="Channel"/>.
        /// </summary>
        BeforeChannelLeave,
        /// <summary>
        ///     This event is raised after an <see cref="User"/> left a <see cref="Channel"/>.
        /// </summary>
        ChannelLeft,
        /// <summary>
        ///     This event is raised before an <see cref="Member"/> leaves a <see cref="Group"/>.
        /// </summary>
        BeforeGroupLeave,
        /// <summary>
        ///     This event is raised after an <see cref="Member"/> left a <see cref="Group"/>.
        /// </summary>
        GroupLeft,
        
        /// <summary>
        ///     This event is raised before a <see cref="Package"/> is handled by the server. This event won't be raised if the package has the type <see cref="PackageType.AES"/>.
        /// </summary>
        BeforePackageReceive,
        /// <summary>
        ///     This event is raised after a <see cref="Package"/> was handled by the server. This event won't be raised if the package has the type <see cref="PackageType.AES"/>.
        /// </summary>
        PackageReceived,

        /// <summary>
        ///     This event is raised before an <see cref="Account"/> is removed.
        /// </summary>
        BeforeAccountRemove,
        /// <summary>
        ///     This event is raised after an <see cref="Account"/> was removed.
        /// </summary>
        AccountRemoved,
        /// <summary>
        ///     This event is raised before a <see cref="Channel"/> is removed.
        /// </summary>
        BeforeChannelRemove,
        /// <summary>
        ///     This event is raised after a <see cref="Channel"/> was removed.
        /// </summary>
        ChannelRemoved,
        /// <summary>
        ///     This event is raised before a <see cref="Group"/> is removed.
        /// </summary>
        BeforeGroupRemove,
        /// <summary>
        ///     This event is raised after a <see cref="Group"/> was removed.
        /// </summary>
        GroupRemoved,

        /// <summary>
        ///     This event is raised when a <see cref="User"/> is typing.
        /// </summary>
        [Obsolete]
        Typing,

        /// <summary>
        ///     This event is raised before a <see cref="User"/> sent an input.
        /// </summary>
        BeforeInput,
        /// <summary>
        ///     This event is raised after a <see cref="User"/> sent an input.
        /// </summary>
        Input,
    }
}
