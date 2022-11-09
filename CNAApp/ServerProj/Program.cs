using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;


namespace ServerProj
{
    internal class Server
    {
        static void Main()
        {
            Server server = new Server("127.0.0.1", 4444);
            server.Start();
            server.Stop();
        }

        TcpListener m_TcpListener;

        public Server(String ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            m_TcpListener = new TcpListener(ip, port);
        }

        public void Start()
        {
            m_TcpListener.Start();

            Console.WriteLine("listening...");

            Socket socket = m_TcpListener.AcceptSocket();
            Console.WriteLine("connection made");
            ClientMethod(socket);
        }

        public void Stop()
        {
            m_TcpListener.Stop();
        }

        private void ClientMethod(Socket socket)
        {
            string recievedMessage;
            NetworkStream stream = new NetworkStream(socket, true);
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

            writer.WriteLine("you have connected to the server");
            writer.Flush();

            while ((recievedMessage == reader.ReadLine()) != null)
            {
                GetReturnMessage(recievedMessage);

                writer.WriteLine(GetReturnMessage(recievedMessage));    
                writer.Flush ();
            }
            socket.Close();
        }

        private string GetReturnMessage(string code)
        {
            return "hello";
        }
    }
}