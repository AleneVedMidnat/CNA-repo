using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
        BinaryWriter m_writer;
        BinaryReader m_reader;
        BinaryFormatter m_formatter;
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
                m_reader = new BinaryReader(m_stream, Encoding.UTF8);
                m_writer = new BinaryWriter(m_stream, Encoding.UTF8);
                m_formatter = new BinaryFormatter();
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
                int numberOfBytes ;
                if ((numberOfBytes = m_reader.ReadInt32()) != -1)
                {
                    byte[] buffer = m_reader.ReadBytes(numberOfBytes);
                    MemoryStream m_memoryStream = new MemoryStream(buffer);
                    Packets.ChatMessagePacket recievedPacket = m_formatter.Deserialize(m_memoryStream) as Packets.ChatMessagePacket;
                    m_form.UpdateChatBox(recievedPacket.m_message);
                }
            }
            //while (tcpClient.Connected)
            //{
            //    m_form.UpdateChatBox("Server says: " + m_reader.ReadLine());
            //}
        }

        public void SendMessage(string message)
        {
            Packets.ChatMessagePacket newPacket = new Packets.ChatMessagePacket(message);
            MemoryStream m_memoryStream = new MemoryStream();
            m_formatter.Serialize(m_memoryStream, newPacket);
            byte[] buffer = m_memoryStream.GetBuffer();
            m_writer.Write(buffer.Length);
            m_writer.Write(buffer);
            m_writer.Flush();
            //m_writer.WriteLine(message);
            //m_writer.Flush();
        }
    }

    
}
