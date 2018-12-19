using Neo.Core.Shared;
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
            Pool.Server.Clients.Add(new Client(ID, Context.WebSocket));
            Pool.Server.OnConnect(ID);
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
            var client = Pool.Server.Clients.Find(c => c.ClientId == ID);

            if (!client.IsOfficial) {
                if (e.Data == "4D303DA2-6A01-483E-89DF-BA01E919FF99") {
                    client.IsOfficial = true;
                    Sessions.SendTo(ID, "7FDB1C16-F94A-4A6C-90C9-47404EC44594");
                    return;
                } else {
                    Pool.Server.Clients.Remove(client);
                    Sessions.CloseSession(ID);
                    return;
                }
            }

            Pool.Server.OnMessage(ID, e.Data);
        }
    }
}
