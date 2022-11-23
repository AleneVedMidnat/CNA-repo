using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace CNAApp
{
    internal class Program
    {
        [STAThread]

        static void Main()
        {
            
            Console.WriteLine("Hello World");

            Client client = new Client();
            if (client.Connect("127.0.0.1", 4444))
            {
                client.Run();
            }
            else
            {
                Console.WriteLine("failed to connect to the server");
            }
        }
    }

    public class Client
    {
        private TcpClient tcpClient;
        NetworkStream m_stream;
        StreamWriter m_writer;
        StreamReader m_reader;
        MainWindow m_form;

        public Client()
        {
            tcpClient = new TcpClient();
        }

        public bool Connect(string ipAddress, int port)
        {
            try
            {
                tcpClient.Connect(ipAddress, port);
                m_stream = tcpClient.GetStream();
                m_reader = new StreamReader(m_stream, Encoding.UTF8);
                m_writer = new StreamWriter(m_stream, Encoding.UTF8);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return false;
            }
        }

        public void Run()
        {
            m_form = new MainWindow(this);
            Thread serverProcessThread = new Thread(() => { ProcessServerResponse(); });
            serverProcessThread.Start();
            m_form.ShowDialog();
        }

        private void ProcessServerResponse()
        {
            while (tcpClient.Connected)
            {
                m_form.UpdateChatBox("Server says: " + m_reader.ReadLine());
            }
        }

        public void SendMessage(string message)
        {
            m_writer.WriteLine(message);
            m_writer.Flush();
        }
    }

    
}
