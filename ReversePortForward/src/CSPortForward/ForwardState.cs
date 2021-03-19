using System.Net;
using System.Net.Sockets;

namespace CSPortForward
{
    public class ForwardState
    {
        public Socket LocalSocket { get; set; }
        public Socket RemoteSocket { get; set; }
        public byte[] LocalBuffer { get; set; }
        public byte[] RemoteBuffer { get; set; }
        public IPEndPoint LocalEndPoint { get; set; }
        public IPEndPoint RemoteEndPoint { get; set; }
        public ForwardState()
        {
            LocalBuffer = new byte[65535];
            RemoteBuffer = new byte[65535];
        }
    }
}
