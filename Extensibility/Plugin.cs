#pragma warning disable 1998
// ReSharper disable UnusedMember.Global

using System;
using System.Threading.Tasks;
using Neo.Core.Communication;
using Neo.Core.Extensibility.Events;
using Neo.Core.Shared;

namespace Neo.Core.Extensibility
{
    public abstract class Plugin
    {
        public Guid InternalId { get; } = Guid.NewGuid();
        public abstract string Namespace { get; }

        public virtual async Task OnInitialize() { }

        public virtual async Task OnCustom(CustomEventArgs args) { }

        public virtual async Task OnConnected(ConnectEventArgs args) { }
        public virtual async Task OnDisconnected(DisconnectEventArgs args) { }

        public virtual async Task OnBeforePackageReceive(Before<ReceiveElementEventArgs<Package>> args) { }
        public virtual async Task OnPackageReceived(ReceiveElementEventArgs<Package> args) { }

        public virtual async Task OnBeforeAccountCreate(Before<CreateElementEventArgs<Account>> args) { }
        public virtual async Task OnAccountCreated(CreateElementEventArgs<Account> args) { }
        public virtual async Task OnBeforeChannelCreate(Before<CreateElementEventArgs<Channel>> args) { }
        public virtual async Task OnChannelCreated(CreateElementEventArgs<Channel> args) { }
        public virtual async Task OnBeforeGroupCreate(Before<CreateElementEventArgs<Group>> args) { }
        public virtual async Task OnGroupCreated(CreateElementEventArgs<Group> args) { }

        public virtual async Task OnBeforeAccountEdit(Before<EditElementEventArgs<Account>> args) { }
        public virtual async Task OnAccountEdited(EditElementEventArgs<Account> args) { }
        public virtual async Task OnBeforeChannelEdit(Before<EditElementEventArgs<Channel>> args) { }
        public virtual async Task OnChannelEdited(EditElementEventArgs<Channel> args) { }
        public virtual async Task OnBeforeGroupEdit(Before<EditElementEventArgs<Group>> args) { }
        public virtual async Task OnGroupEdited(EditElementEventArgs<Group> args) { }
        public virtual async Task OnBeforeIdentityEdit(Before<EditElementEventArgs<Identity>> args) { }
        public virtual async Task OnIdentityEdited(EditElementEventArgs<Identity> args) { }

        public virtual async Task OnBeforeChannelJoin(Before<JoinElementEventArgs<Channel>> args) { }
        public virtual async Task OnChannelJoined(JoinElementEventArgs<Channel> args) { }
        public virtual async Task OnBeforeGroupJoin(Before<JoinElementEventArgs<Group>> args) { }
        public virtual async Task OnGroupJoined(JoinElementEventArgs<Group> args) { }

        public virtual async Task OnBeforeChannelLeave(Before<LeaveElementEventArgs<Channel>> args) { }
        public virtual async Task OnChannelLeft(LeaveElementEventArgs<Channel> args) { }
        public virtual async Task OnBeforeGroupLeave(Before<LeaveElementEventArgs<Group>> args) { }
        public virtual async Task OnGroupLeft(LeaveElementEventArgs<Group> args) { }

        public virtual async Task OnBeforeAccountRemove(Before<RemoveElementEventArgs<Account>> args) { }
        public virtual async Task OnAccountRemoved(RemoveElementEventArgs<Account> args) { }
        public virtual async Task OnBeforeChannelRemove(Before<RemoveElementEventArgs<Channel>> args) { }
        public virtual async Task OnChannelRemoved(RemoveElementEventArgs<Channel> args) { }
        public virtual async Task OnBeforeGroupRemove(Before<RemoveElementEventArgs<Group>> args) { }
        public virtual async Task OnGroupRemoved(RemoveElementEventArgs<Group> args) { }

        public virtual async Task OnTyping(TypingEventArgs args) { }
    }

    public class P : Plugin
    {
        public override string Namespace { get; }
    }
}
 