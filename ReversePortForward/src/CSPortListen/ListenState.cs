using System.Net.Sockets;

namespace CSPortListen
{
    /// <summary>
    /// socket listen state
    /// </summary>
    public class ListenState
    {
        public Socket ClientSocket;
        public Socket UserSocket;
        public byte[] ClientBuffer;
        public byte[] UserBuffer;
        public const int Lenght = 65535;
        public ListenState()
        {
            ClientBuffer = new byte[Lenght];
            UserBuffer = new byte[Lenght];
        }
    }
}
