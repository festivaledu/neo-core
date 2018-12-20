using System.Threading.Tasks;
using Neo.Core.Communication;
using Neo.Core.Shared;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Neo.Core.Networking
{
    /// <summary>
    ///     <see cref="WebSocketBehavior"/> used to redirect all <see cref="WebSocket"/> events to <see cref="Pool.Server"/>.
    /// </summary>
    internal class NeoWebSocketBehaviour : WebSocketBehavior
    {
        protected override void OnOpen() {
            Pool.Server.OnConnect(new Client(ID, Context.WebSocket));
            Pool.Server.SessionManager = Sessions;
        }

        protected override void OnClose(CloseEventArgs e) {
            Pool.Server.OnDisconnect(ID, e.Code, e.Reason, e.WasClean);
            Pool.Server.SessionManager = Sessions;
        }

        protected override void OnError(ErrorEventArgs e) {
            Pool.Server.OnError(ID, e.Exception, e.Message);
            Pool.Server.SessionManager = Sessions;
        }

        protected override void OnMessage(MessageEventArgs e) {
            Container container;

            try {
                container = JsonConvert.DeserializeObject<Container>(e.Data);
            } catch {
                // TODO: Close connection
                return;
            }

            Task.Run(async () => {
                var package = await Pool.Server.Clients.Find(c => c.ClientId == ID).ReadContainer(container);

                if (package.Type == PackageType.Aes) {
                    // TODO: Load cryptographic data
                } else {
                    Pool.Server.OnPackage(ID, package);
                }
            });
        }
    }
}
