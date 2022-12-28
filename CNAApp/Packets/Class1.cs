using System.Security.Cryptography;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Packets
{
    [Serializable]
    public class Packet
    {
        public enum PacketType
        {
            ChatMessage,
            RSAMessage
        }

        public PacketType m_packetType { get; protected set; }
    }

    [Serializable]
    public class ChatMessagePacket : Packet
    {
        public byte[] m_message;

        public ChatMessagePacket(byte[] message)
        {
            m_message = message;
            m_packetType = PacketType.ChatMessage;
        }
    }

    [Serializable]
    public class RSAPacket : Packet
    {
        public string m_key;

        public RSAPacket(RSAParameters key)
        {
            m_packetType = PacketType.RSAMessage;

            StringWriter m_writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(RSAParameters));
            serializer.Serialize(m_writer, key);
            m_key = m_writer.ToString();
        }
    }
}