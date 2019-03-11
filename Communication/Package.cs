using Neo.Core.Communication.Packages;
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
}
