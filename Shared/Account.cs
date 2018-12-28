using System;
using System.Collections.Generic;

namespace Neo.Core.Shared
{
    public class Account
    {
        public string Email { get; set; }
        public Guid InternalId { get; private set; }
        public byte[] Password { get; set; }
        
        // TODO
        public List<Group> Groups { get; } => P
    }
}
