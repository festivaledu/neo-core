﻿using System;

namespace Neo.Core.Shared
{
    public class Member : User
    {
        // TODO
        public Account Account { get; set; }

        public new Guid InternalId => Account.InternalId;
    }
}