using Neo.Core.Shared;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Neo.Core.Networking
{
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
            Pool.Server.OnMessage(ID, e.Data);
        }
    }
}
