using System.Diagnostics;
using System.Threading;

namespace InteractiveCMD
{
    public class ThreadMain
    {
        Thread _thread;
        public ThreadMain()
        {
            _thread = new Thread(new ThreadStart(Run));
            _thread.Start();
        }

        void Run()
        {

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "C:\\windows\\system32\\cmd.exe",
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    ErrorDialog = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            var writer = process.StandardInput;
            var reader = process.StandardOutput;
            var error = process.StandardError;
            var form = new InterActiveForm()
            {
                Writer = writer,
                Reader = reader,
                Error = error
            };
            form.ShowDialog();
        }
    }
}
