using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace CSPortListen
{

    /// <summary>
    /// listen
    /// </summary>
    static class Program
    {
        [STAThread]
        static void Main(String[] args)
        {
            Application.Run(new MainForm());
        }
    }
}
