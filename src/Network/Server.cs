using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using DefaultNamespace;

namespace PokemonViewer.Network
{
    public class Server
    {
        public HttpListener server { get; private set; }
        public int timeouts { get; private set; }
        private byte[] buffer;


        // private GameData? dataToSend;
        private bool isBlocked;
        
        public Server(int port = 13000)
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            server = new HttpListener();
            isBlocked = false;
            // dataToSend = null;
            server.TimeoutManager.IdleConnection = TimeSpan.FromSeconds(1);
            server.Prefixes.Add($"http://localhost:{port}/");
            server.Start();
            buffer = new byte[256];
        }

        public async Task<int> SendData(GameData gameData)
        {
            if (isBlocked)
                return 1;

            isBlocked = true;
            await Task.Run(() =>
            {
                SendDataAsync(gameData);
            });
            isBlocked = false;
            return 0;
        }

        private void SendDataAsync(GameData gameData)
        {
            HttpListenerContext context = server.GetContext(); //TODO maybe I should use BeginGetContext instead
            var request = context.Request;
            var response = context.Response;
            var output = response.OutputStream;
            
            response.Headers.Add(HttpResponseHeader.ContentType,"text/json");

            byte[] buffer = Encoding.UTF8.GetBytes(gameData.Serialize());
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
    }
}