using System.Security.Cryptography;
using System.Threading.Tasks;
using Neo.Core.Communication;
using Neo.Core.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Neo.Core.Networking
{
    /// <summary>
    ///     Represents a client connected to a <see cref="BaseServer"/>.
    /// </summary>
    public class Client
    {
        /// <summary>
        ///     Id used by the <see cref="WebSocketSessionManager"/> to identify the <see cref="IWebSocketSession"/> this <see cref="Socket"/> belongs to.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        ///     <see cref="WebSocket"/> used for sending and receiving data.
        /// </summary>
        [JsonIgnore]
        public WebSocket Socket { get; set; }

        private AesParameters aesParameters;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="clientId">Id used by the <see cref="WebSocketSessionManager"/> to identify the <see cref="IWebSocketSession"/> this <see cref="Socket"/> belongs to.</param>
        /// <param name="socket"><see cref="WebSocket"/> used for sending and receiving data.</param>
        internal Client(string clientId, WebSocket socket) {
            this.ClientId = clientId;
            this.Socket = socket;

            var wssv = new WebSocketServer("ws://localhost:42421");
            wssv.AddWebSocketService<NeoWebSocketBehaviour>("/neo");
        }

        /// <summary>
        ///     Reads a package asynchronously from a container.
        /// </summary>
        /// <param name="data">The container to read from.</param>
        /// <returns>Returns a <see cref="Task"/> that represents the asynchronous read operation. The value of the <c>TResult</c> parameter contains the read <see cref="Package"/>.</returns>
        /// <remarks>If the container is encrypted, the returned <see cref="Package"/> will be decrypted automatically.</remarks>
        internal Package ReadContainer(Container data) {
            Package package;

            if (data.IsEncrypted) {
                var decrypted = NeoCryptoProvider.Instance.AesDecrypt(data.Payload, aesParameters);
                package = JsonConvert.DeserializeObject<Package>(decrypted);
            } else {
                package = JsonConvert.DeserializeObject<Package>(data.Payload);
            }

            return package;
        }

        /// <summary>
        ///     Sends a package asynchronously through a container.
        /// </summary>
        /// <param name="data">The package to send.</param>
        /// <param name="encrypt">A boolean value determining whether the package should be encrypted.</param>
        internal void SendPackage(Package data, bool encrypt = true) {
            Container container;

            if (encrypt) {
                if (!aesParameters.IsValid()) {
                    throw new CryptographicException("No AES parameters are set.");
                }

                var dataJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                var encrypted = NeoCryptoProvider.Instance.AesEncrypt(dataJson, aesParameters);
                container = new Container(true, encrypted);
            } else {
                var dataJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                container = new Container(false, dataJson);
            }

            var containerJson = JsonConvert.SerializeObject(container, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            Socket.Send(containerJson);
        }

        /// <summary>
        ///     Sends <see cref="RSAParameters"/> synchronously.
        /// </summary>
        /// <param name="parameters">The parameters to send.</param>
        // ReSharper disable once InconsistentNaming
        public void SendRSAParameters(RSAParameters parameters) {
            var container = new Container(false, JsonConvert.SerializeObject(new Package(PackageType.RSA, parameters), new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
            Socket.Send(JsonConvert.SerializeObject(container, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }

        /// <summary>
        ///     Sets the <see cref="AesParameters"/> of this <see cref="Client"/>.
        /// </summary>
        /// <param name="parameters">The <see cref="AesParameters"/> structure to set.</param>
        internal void SetAesParameters(AesParameters parameters) {
            aesParameters = parameters;
        }
    }
}
