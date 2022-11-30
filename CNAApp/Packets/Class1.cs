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
    }

    [Serializable]
    public class ChatMessagePacket : Packet
    {
        public string m_message;

        public ChatMessagePacket(string message)
        {
            m_message = message;
            m_packetType = PacketType.ChatMessage;
        }
    }
}