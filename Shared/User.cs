using System;
using System.Collections.Generic;
using Neo.Core.Attribution;
using Neo.Core.Authorization;
using Neo.Core.Networking;
using Newtonsoft.Json;

namespace Neo.Core.Shared
{
    /// <summary>
    ///     Represents a single person using the client.
    /// </summary>
    public abstract class User : IAttributable, IAuthorizable
    {
        /// <summary>
        ///     The <see cref="Channel"/> this <see cref="User"/> has currently opened.
        /// </summary>
        [JsonIgnore]
        public Channel ActiveChannel => Pool.Server.Channels.Find(c => c.ActiveMemberIds.Contains(InternalId));

        /// <summary>
        ///     The attributes assigned to this <see cref="User"/>.
        /// </summary>
        public virtual Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
        
        /// <summary>
        ///     Contains all <see cref="Channel"/>s this <see cref="User"/> is a member of.
        /// </summary>
        [JsonIgnore]
        public List<Channel> Channels => Pool.Server.Channels.FindAll(c => c.MemberIds.Contains(InternalId));
        
        /// <summary>
        ///     The <see cref="Networking.Client"/> associated with this <see cref="User"/>.
        /// </summary>
        public Client Client { get; set; }
        
        /// <summary>
        ///     The <see cref="Identity"/> of this <see cref="User"/>.
        /// </summary>
        public virtual Identity Identity { get; set; }

        /// <summary>
        ///     The unique id used to identify this <see cref="User"/>.
        /// </summary>
        public virtual Guid InternalId { get; set; } = Guid.NewGuid();
        
        /// <summary>
        ///     The permissions granted to this <see cref="User"/>.
        /// </summary>
        public virtual Dictionary<string, Permission> Permissions { get; set; } = new Dictionary<string, Permission>();


        /// <summary>
        ///     Returns a <see cref="Target"/> with this <see cref="User"/>.
        /// </summary>
        /// <returns>Returns the created <see cref="Target"/>.</returns>
        public Target ToTarget() {
            return new Target(this);
        }
    }
}
