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
            TcpClient tcpClient = new TcpClient();
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
                m_writer.WriteLine();
                m_writer.Flush();

                ProcessServerResponse();
                if (userInput == )
            }
        }

        private void ProcessServerResponse()
        {

        }
    }
}
