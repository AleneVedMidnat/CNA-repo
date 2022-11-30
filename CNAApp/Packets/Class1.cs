using System.Security.Cryptography;

namespace Packets
{
    [Serializable]
    public class Packet
    {
        public enum PacketType
        {
            ChatMessage,
            PrivateMessage,
            ClientName
        }

        public PacketType m_packetType { get; protected set; }
        public RSAParameters m_key;
    }

    [Serializable]
    public class ChatMessagePacket : Packet
    {
        public string m_message;

        public ChatMessagePacket(string message, RSAParameters key)
        {
            m_message = message;
            m_packetType = PacketType.ChatMessage;
            m_key = key;
        }
    }
}