#pragma warning disable 1998
// ReSharper disable UnusedMember.Global

using System;
using Neo.Core.Communication;
using Neo.Core.Extensibility.Events;
using Neo.Core.Networking;
using Neo.Core.Shared;

namespace Neo.Core.Extensibility
{
    /// <summary>
    ///     Represents the base class for an extensible component.
    /// </summary>
    public abstract class Plugin
    {
        /// <summary>
        ///     The unique id used to identify this <see cref="Plugin"/>.
        /// </summary>
        public Guid InternalId { get; } = Guid.NewGuid();

        /// <summary>
        ///     The namespace of this <see cref="Plugin"/>.
        /// </summary>
        public abstract string Namespace { get; }


        public virtual void OnInitialize(string storagePath) { }
        public virtual void OnDispose() { }

        public virtual void OnCustom(CustomEventArgs args) { }

        public virtual void OnServerInitialized(BaseServer server) { }

        public virtual void OnConnected(ConnectEventArgs args) { }
        public virtual void OnDisconnected(DisconnectEventArgs args) { }
        public virtual void OnServerJoined(JoinElementEventArgs<BaseServer> args) { }
        public virtual void OnLogin(LoginEventArgs args) { }
        public virtual void OnServerLeft(LeaveElementEventArgs<BaseServer> args) { }

        public virtual void OnBeforePackageReceive(Before<ReceiveElementEventArgs<Package>> args) { }
        public virtual void OnPackageReceived(ReceiveElementEventArgs<Package> args) { }

        public virtual void OnBeforeAccountCreate(Before<CreateElementEventArgs<Account>> args) { }
        public virtual void OnAccountCreated(CreateElementEventArgs<Account> args) { }
        public virtual void OnBeforeChannelCreate(Before<CreateElementEventArgs<Channel>> args) { }
        public virtual void OnChannelCreated(CreateElementEventArgs<Channel> args) { }
        public virtual void OnBeforeGroupCreate(Before<CreateElementEventArgs<Group>> args) { }
        public virtual void OnGroupCreated(CreateElementEventArgs<Group> args) { }

        public virtual void OnBeforeAccountEdit(Before<EditElementEventArgs<Account>> args) { }
        public virtual void OnAccountEdited(EditElementEventArgs<Account> args) { }
        public virtual void OnBeforeChannelEdit(Before<EditElementEventArgs<Channel>> args) { }
        public virtual void OnChannelEdited(EditElementEventArgs<Channel> args) { }
        public virtual void OnBeforeGroupEdit(Before<EditElementEventArgs<Group>> args) { }
        public virtual void OnGroupEdited(EditElementEventArgs<Group> args) { }
        public virtual void OnBeforeIdentityEdit(Before<EditElementEventArgs<Identity>> args) { }
        public virtual void OnIdentityEdited(EditElementEventArgs<Identity> args) { }

        public virtual void OnBeforeChannelJoin(Before<JoinElementEventArgs<Channel>> args) { }
        public virtual void OnChannelJoined(JoinElementEventArgs<Channel> args) { }
        public virtual void OnBeforeGroupJoin(Before<JoinElementEventArgs<Group>> args) { }
        public virtual void OnGroupJoined(JoinElementEventArgs<Group> args) { }

        public virtual void OnBeforeChannelLeave(Before<LeaveElementEventArgs<Channel>> args) { }
        public virtual void OnChannelLeft(LeaveElementEventArgs<Channel> args) { }
        public virtual void OnBeforeGroupLeave(Before<LeaveElementEventArgs<Group>> args) { }
        public virtual void OnGroupLeft(LeaveElementEventArgs<Group> args) { }

        public virtual void OnBeforeAccountRemove(Before<RemoveElementEventArgs<Account>> args) { }
        public virtual void OnAccountRemoved(RemoveElementEventArgs<Account> args) { }
        public virtual void OnBeforeChannelRemove(Before<RemoveElementEventArgs<Channel>> args) { }
        public virtual void OnChannelRemoved(RemoveElementEventArgs<Channel> args) { }
        public virtual void OnBeforeGroupRemove(Before<RemoveElementEventArgs<Group>> args) { }
        public virtual void OnGroupRemoved(RemoveElementEventArgs<Group> args) { }

        [Obsolete]
        public virtual void OnTyping(TypingEventArgs args) { }

        public virtual void OnBeforeInput(Before<InputEventArgs> args) { }
        public virtual void OnInput(InputEventArgs args) { }
    }
}
 