using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Client
{

    public class Server
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            
            TcpListener server = new TcpListener(IPAddress.Any, 7080);

            try
            {
                server.Start();
                Console.WriteLine("Server has started!");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("New client connected!");
                    Thread clientThread = new Thread(() => handleClient(client));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught in server: {ex.Message} - ST: {ex.StackTrace}");
            }
        }

        static void handleClient(TcpClient client)
        {
            #region Get client request
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);
            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received: {dataReceived}");
            #endregion

            #region Simulated stress
            Console.WriteLine("Simulating stress...");
            Thread.Sleep(TimeSpan.FromSeconds(10));
            #endregion

            #region Reply to the request
            byte[] dataToSend = Encoding.ASCII.GetBytes($"Hello, Client! you said {dataReceived}");
            stream.Write(dataToSend, 0, dataToSend.Length);
            Console.WriteLine("Data sent!");
            #endregion

            #region Close connection
            client.Close();
            Console.WriteLine("Client disconnected!");
            #endregion
        }
    }
}
