using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace CNAApp
{
    internal class Program
    {
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
            string userInput;
            ProcessServerResponse();

            while ((userInput = Console.ReadLine()) != null)
            {
                m_writer.WriteLine(userInput);
                m_writer.Flush();

                ProcessServerResponse();
                if (userInput == "Close") { break;  }
            }
            tcpClient.Close();
        }

        private void ProcessServerResponse()
        {
            Console.WriteLine("Server says: " + m_reader.ReadLine());
            Console.WriteLine();
        }
    }
}
