using System;
using System.Threading.Tasks;
using Neo.Core.Communication;
using Neo.Core.Cryptography;
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
            Task.Run(async () => {
                await Pool.Server.OnConnect(new Client(ID, Context.WebSocket));
                Pool.Server.SessionManager = Sessions;
            });
        }

        protected override void OnClose(CloseEventArgs e) {
            Task.Run(async () => {
                await Pool.Server.OnDisconnect(ID, e.Code, e.Reason, e.WasClean);
                Pool.Server.SessionManager = Sessions;
            });
        }

        protected override void OnError(ErrorEventArgs e) {
            Task.Run(async () => {
                await Pool.Server.OnError(ID, e.Exception, e.Message);
                Pool.Server.SessionManager = Sessions;
            });
        }

        protected override void OnMessage(MessageEventArgs e) {
            Container container;

            try {
                container = JsonConvert.DeserializeObject<Container>(e.Data);
            } catch {
                Pool.Server.Clients.RemoveAll(c => c.ClientId == ID);
                Sessions.CloseSession(ID);
                return;
            }

            var client = Pool.Server.Clients.Find(c => c.ClientId == ID);
            var package = client.ReadContainer(container).Result;

            if (package.Type == PackageType.AES) {
                // TODO: Add RSA decryption
                //client.SetAesParameters(JsonConvert.DeserializeObject<AesParameters>(NeoCryptoProvider.Instance.RsaDecrypt(package.Content, Pool.Server.RSAPrivateParameters)));

                var payload = package.GetContentTypesafe<AesPackagePayload>();
                var parameters = new AesParameters(Convert.FromBase64String(payload.AesKey), Convert.FromBase64String(payload.AesIV));
                client.SetAesParameters(parameters);
            } else {
                Pool.Server.OnPackage(ID, package);
            }
        }
    }
}
