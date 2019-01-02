namespace Neo.Core.Extensibility.Events
{
    public enum EventType
    {
        Custom,

        BeforeAccountCreate,
        AccountCreated,
        BeforeChannelCreate,
        ChannelCreated,
        BeforeGroupCreate,
        GroupCreated,

        BeforeAccountEdit,
        AccountEdited,
        BeforeChannelEdit,
        ChannelEdited,
        BeforeGroupEdit,
        GroupEdited,
        BeforeIdentityEdit,
        IdentityEdited,

        BeforeChannelJoin,
        ChannelJoined,
        BeforeGroupJoin,
        GroupJoined,

        BeforeChannelLeave,
        ChannelLeft,
        BeforeGroupLeave,
        GroupLeft,

        BeforeAccountRemove,
        AccountRemoved,
        BeforeChannelRemove,
        ChannelRemoved,
        BeforeGroupRemove,
        GroupRemoved,
    }
}
