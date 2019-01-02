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
        BeforeServerEdit,
        ServerEdited,

        BeforeChannelJoin,
        ChannelJoined,
        BeforeGroupJoin,
        GroupJoined,
        BeforeServerJoin,
        ServerJoined,

        BeforeChannelLeave,
        ChannelLeft,
        BeforeGroupLeave,
        GroupLeft,
        BeforeServerLeave,
        ServerLeft
    }
}
