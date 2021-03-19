using System;
using System.Windows.Forms;

namespace InteractiveCMD
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            new ThreadMain(); 
            Application.Run();
        }
    }
}
