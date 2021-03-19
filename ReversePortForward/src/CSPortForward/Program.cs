using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;

namespace CSPortForward
{

    /// <summary>
    /// froward
    /// </summary>
    static class Program
    {
        [STAThread]
        static void Main(String[] args)
        {
            try
            {
#if DEBUG
                Debug.Listeners.Add(new ConsoleTraceListener(true));
#endif
                new TcpForwardSlim().Start(
                    new IPEndPoint(IPAddress.Parse(args[0]), int.Parse(args[1])),
                    new IPEndPoint(IPAddress.Parse(args[2]), int.Parse(args[3])));
                Application.Run();
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#endif
            }
        }
    }
}
