using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading.Channels;
using System.Collections.Concurrent;

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
        ConcurrentDictionary<int, ConnectedClient> m_clients;

        public Server(String ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            m_TcpListener = new TcpListener(ip, port);
        }

        public void Start()
        {
            m_TcpListener.Start();
            m_clients = new ConcurrentDictionary<int, ConnectedClient>();
            int clientIndex = 0;

            while (true)
            {
                Console.WriteLine("listening...");
                Socket socket = m_TcpListener.AcceptSocket();
                ConnectedClient client = new ConnectedClient(socket); ;
                Console.WriteLine("connection made");
                int index = clientIndex;
                clientIndex++;
                m_clients.TryAdd(index, client);
                Thread thread = new Thread(() => { ClientMethod(index); });
                thread.Start();
            }
            
        }

        public void Stop()
        {
            m_TcpListener.Stop();
        }

        private void ClientMethod(int index)
        {
            string recievedMessage;  
            ConnectedClient client = m_clients[index];

            while ((recievedMessage = client.Read()) != null)
            {
                GetReturnMessage(recievedMessage);

                client.Send(recievedMessage);
            }
            m_clients[index].Close();
            ConnectedClient c;
            m_clients.TryRemove(index, out c);
        }

        private string GetReturnMessage(string code)
        {
            return "hello";
        }
    }

    internal class ConnectedClient 
    {
        private Socket m_socket;
        private NetworkStream m_stream;
        private StreamReader m_reader;
        private StreamWriter m_writer;
        private object m_readLock;
        private object m_writeLock;

        public ConnectedClient(Socket socket)
        {
            m_writeLock = new object();
            m_readLock = new object();
            m_socket = socket;
            m_stream = new NetworkStream(socket, true);
            m_reader = new StreamReader(m_stream, Encoding.UTF8);
            m_writer = new StreamWriter(m_stream, Encoding.UTF8);
        }

        public void Close()
        {
            m_stream.Close();
            m_reader.Close();
            m_writer.Close();
            m_socket.Close();
        }

        public string Read()
        {
            lock (m_readLock)
            {
                return m_reader.ReadLine();
            }
        }

        public void Send(string message)
        {
            lock (m_writeLock)
            {
                m_writer.WriteLine(message);
                m_writer.Flush();
            }

        }
    }

}