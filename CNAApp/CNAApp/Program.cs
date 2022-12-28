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
using System.Security.Cryptography;
using Packets;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Windows.Input;

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
        RSACryptoServiceProvider m_RSAProvider;
        RSAParameters m_PublicKey;
        RSAParameters m_PrivateKey;
        RSAParameters m_ServerKey;

        public Client()
        {
            tcpClient = new TcpClient();
            m_RSAProvider = new RSACryptoServiceProvider(1024);
            m_PublicKey = m_RSAProvider.ExportParameters(false);
            m_PrivateKey = m_RSAProvider.ExportParameters(true);
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
                SendRSAKey();
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

                    Packets.Packet recievedMessage = m_formatter.Deserialize(m_memoryStream) as Packets.Packet;
                    switch (recievedMessage.m_packetType)
                    {
                        case Packets.Packet.PacketType.ChatMessage:
                            Packets.ChatMessagePacket chatPacket = (Packets.ChatMessagePacket)recievedMessage;
                            m_form.UpdateChatBox(DecryptString(chatPacket.m_message));
                            break;
                        case Packets.Packet.PacketType.RSAMessage:
                            Packets.RSAPacket rsapacket = (Packets.RSAPacket)recievedMessage;
                            m_ServerKey = FromXml(rsapacket.m_key);
                            break;
                    }
                }
            }
        }

        public void SendMessage(string message)
        {
            Packets.ChatMessagePacket newPacket = new Packets.ChatMessagePacket(EncryptString(message));
            MemoryStream m_memoryStream = new MemoryStream();
            m_formatter.Serialize(m_memoryStream, newPacket);
            byte[] buffer = m_memoryStream.GetBuffer();
            m_writer.Write(buffer.Length);
            m_writer.Write(buffer);
            m_writer.Flush();
        }

        private void SendRSAKey()
        {
            Packets.RSAPacket newPacket = new Packets.RSAPacket(m_PrivateKey);
            MemoryStream m_memoryStream = new MemoryStream();
            m_formatter.Serialize(m_memoryStream, newPacket);
            byte[] buffer = m_memoryStream.GetBuffer();
            m_writer.Write(buffer.Length);
            m_writer.Write(buffer);
            m_writer.Flush();

        }


        private byte[] Encrypt(byte[] data)
        {
            lock (m_RSAProvider);
            m_RSAProvider.ImportParameters(m_PublicKey);
            return m_RSAProvider.Encrypt(data, true);
        }

        private byte[] Decrypt(byte[] data)
        {
            lock (m_RSAProvider);
            m_RSAProvider.ImportParameters(m_ServerKey);
            return m_RSAProvider.Decrypt(data, true);
        }

        private byte[] EncryptString(string message)
        {
            byte[] byteArray;
            byteArray = Encoding.UTF8.GetBytes(message);
            return Encrypt(byteArray);
        }

        private string DecryptString(byte[] message)
        {
            message = Decrypt(message);
            return Encoding.UTF8.GetString(message);
        }

        public static RSAParameters FromXml(string key)
        {
            StringReader reader = new StringReader(key);
            XmlSerializer serializer = new XmlSerializer(typeof(RSAParameters));
            return (RSAParameters)serializer.Deserialize(reader);
        }
    }

    
}
