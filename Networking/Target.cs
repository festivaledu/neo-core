using System.Collections.Generic;
using System.Linq;
using Neo.Core.Communication;
using Neo.Core.Shared;

namespace Neo.Core.Networking
{
    /// <summary>
    ///     Represents one or more recipients.
    /// </summary>
    public class Target
    {
        /// <summary>
        ///     Targets all connected clients.
        /// </summary>
        public static Target All => new Target { targets = Pool.Server.Clients.Select(c => c.ClientId).ToList() };

        /// <summary>
        ///     Returns a disctinct list of client ids.
        /// </summary>
        public List<string> Targets => targets.Distinct().ToList();

        private List<string> targets = new List<string>();


        /// <summary>
        ///     Initializes a new instance of the <see cref="Target"/> class.
        /// </summary>
        public Target() { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Target"/> class with a recipient.
        /// </summary>
        /// <param name="clientId">The client id of the recipient.</param>
        public Target(string clientId) {
            targets.Add(clientId);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Target"/> class with a recipient.
        /// </summary>
        /// <param name="user">The recipient.</param>
        public Target(User user) {
            targets.Add(user.Client.ClientId);
        }

        
        /// <summary>
        ///     Adds a recipient to the list of recipients.
        /// </summary>
        /// <param name="clientId">The client id of the recipient to add.</param>
        /// <returns>Returns itself to allow chaining.</returns>
        public Target Add(string clientId) {
            targets.Add(clientId);
            return this;
        }

        /// <summary>
        ///     Adds a recipient to the list of recipients.
        /// </summary>
        /// <param name="user">The recipient to add.</param>
        /// <returns>Returns itself to allow chaining.</returns>
        public Target Add(User user) {
            targets.Add(user.Client.ClientId);
            return this;
        }

        /// <summary>
        ///     Adds multiple recipients to the list of recipients.
        /// </summary>
        /// <param name="clientIds">The client ids of the recipients to add.</param>
        /// <returns>Returns itself to allow chaining.</returns>
        public Target AddMany(params string[] clientIds) {
            targets.AddRange(clientIds);
            return this;
        }

        /// <summary>
        ///     Adds multiple recipients to the list of recipients.
        /// </summary>
        /// <param name="users">The recipients to add.</param>
        /// <returns>Returns itself to allow chaining.</returns>
        public Target AddMany(params User[] users) {
            targets.AddRange(users.Select(u => u.Client.ClientId));
            return this;
        }

        /// <summary>
        ///     Adds all active members of a <see cref="Channel"/> as recipients to the list of recipients.
        /// </summary>
        /// <param name="channel">The <see cref="Channel"/> whose active members should be added.</param>
        /// <returns>Returns itself to allow chaining.</returns>
        public Target AddMany(Channel channel) {
            targets.AddRange(channel.ActiveMembers.Select(m => m.Client.ClientId));
            return this;
        }

        /// <summary>
        ///     Removes a recipient from the list of recipients.
        /// </summary>
        /// <param name="clientId">The client id of the recipient to remove.</param>
        /// <returns>Returns itself to allow chaining.</returns>
        public Target Remove(string clientId) {
            targets.Remove(clientId);
            return this;
        }

        /// <summary>
        ///     Removes a recipient from the list of recipients.
        /// </summary>
        /// <param name="user">The recipient to remove.</param>
        /// <returns>Returns itself to allow chaining.</returns>
        public Target Remove(User user) {
            targets.Remove(user.Client.ClientId);
            return this;
        }

        /// <summary>
        ///     Removes multiple recipients from the list of recipients.
        /// </summary>
        /// <param name="clientIds">The client ids of the recipients to remove.</param>
        /// <returns>Returns itself to allow chaining.</returns>
        public Target RemoveMany(params string[] clientIds) {
            targets.RemoveAll(clientIds.Contains);
            return this;
        }

        /// <summary>
        ///     Removes multiple recipients from the list of recipients.
        /// </summary>
        /// <param name="users">The recipients to remove.</param>
        /// <returns>Returns itself to allow chaining.</returns>
        public Target RemoveMany(params User[] users) {
            targets.RemoveAll(t => users.Select(u => u.Client.ClientId).Contains(t));
            return this;
        }

        /// <summary>
        ///     Sends a <see cref="Package"/> to all recipients.
        /// </summary>
        /// <param name="package">The <see cref="Package"/> to send.</param>
        public void SendPackage(Package package) {
            Pool.Server.SendPackageTo(this, package);
        }
    }
}
