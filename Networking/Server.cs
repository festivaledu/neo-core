using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Neo.Core.Networking
{
    internal interface IServer
    {
        List<Client> Clients { get; }
        WebSocketSessionManager SessionManager { get; set; }

        void OnConnect(Client client);
        void OnDisconnect(string clientId, CloseEventArgs args);
        void OnError(string clientId, ErrorEventArgs args);
        void OnMessage(string clientId, MessageEventArgs args);
    }
}
