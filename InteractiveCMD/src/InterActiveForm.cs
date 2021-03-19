using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace InteractiveCMD
{
    public partial class InterActiveForm : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.None)]
        private static extern IntPtr SendMessage(IntPtr hwnd, int wmsg, IntPtr wParam, IntPtr lParam);
        private const int WM_VSCROLL = 277;
        private const int SB_PAGEBOTTOM = 7;
        private IContainer components = null;
        private RichTextBox resultTextbox;
        private TextBox SendBox;
        private StringBuilder builder;

        /// <summary>
        /// WINAPI to scrol to end
        /// </summary>
        /// <param name="richTextbox"></param>
        internal static void ScrollToBottom(RichTextBox richTextbox)
        {
            SendMessage(richTextbox.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
            richTextbox.SelectionStart = richTextbox.Text.Length;
        }
        
        /// <summary>
        /// constractor
        /// </summary>
        public InterActiveForm()
        {
            InitForm();

            this.builder = new StringBuilder();
            this.Load += InterActiveForm_Load;
        }

        private void InitForm()
        {
            this.resultTextbox = new System.Windows.Forms.RichTextBox();
            this.SendBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // resultTextbox
            // 
            this.resultTextbox.Location = new System.Drawing.Point(12, 12);
            this.resultTextbox.Name = "resultTextbox";
            this.resultTextbox.ReadOnly = true;
            this.resultTextbox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.resultTextbox.Size = new System.Drawing.Size(501, 333);
            this.resultTextbox.TabIndex = 0;
            this.resultTextbox.TabStop = false;
            this.resultTextbox.Text = "";
            // 
            // SendBox
            // 
            this.SendBox.AcceptsReturn = true;
            this.SendBox.AcceptsTab = true;
            this.SendBox.Location = new System.Drawing.Point(12, 352);
            this.SendBox.Name = "SendBox";
            this.SendBox.Size = new System.Drawing.Size(501, 20);
            this.SendBox.TabIndex = 1;
            this.SendBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendBox_KeyDown);
            // 
            // InterActiveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 387);
            this.Controls.Add(this.SendBox);
            this.Controls.Add(this.resultTextbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InterActiveForm";
            this.Text = "Interactive Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InterActiveForm_Load(object sender, EventArgs e)
        {
            var reader_thread = new Thread(ReaderListener);
            reader_thread.Start();

            var error_thread = new Thread(ErrorListener);
            error_thread.Start();
        }

        /// <summary>
        /// error thread
        /// </summary>
        void ErrorListener()
        {
            string ch = "";
            while (true)
            {
                if (Error == null) continue;
                if (CloseConnection(Error)) break;
                ch = Convert.ToChar(Error.Read()).ToString();
                AppendString(ch);
            }
        }

        /// <summary>
        /// reader thread
        /// </summary>
        void ReaderListener()
        {
            string ch = "";
            while (true)
            {
                if (Reader == null) continue;
                if (CloseConnection(Reader)) break;
                ch = Convert.ToChar(Reader.Read()).ToString();
                AppendString(ch);
            }
        }

        /// <summary>
        /// close if stream ended
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private bool CloseConnection(StreamReader stream)
        {
            if (stream.EndOfStream)
            {
                builder.AppendLine("Error : connection lost\n\r");
                resultTextbox.BeginInvoke(new MethodInvoker(() =>
                {
                    resultTextbox.Text = builder.ToString();
                    ScrollToBottom(resultTextbox);
                }));
                return true;
            }
            return false;
        }

        /// <summary>
        /// append string to richtextbox
        /// </summary>
        /// <param name="ch"></param>
        private void AppendString(string ch)
        {
            lock (builder)
            {
                builder.Append(ch);
                resultTextbox.BeginInvoke(new MethodInvoker(() =>
                {
                    resultTextbox.Text = builder.ToString();
                    ScrollToBottom(resultTextbox);
                }));
            }
        }

        /// <summary>
        /// reader handler
        /// </summary>
        public StreamReader Reader { get; set; }
        /// <summary>
        /// writer handler
        /// </summary>
        public StreamWriter Writer { get; set; }

        /// <summary>
        /// error handler 
        /// </summary>
        public StreamReader Error { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Writer.WriteLine(this.SendBox.Text);
                this.SendBox.Text = "";
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            Reader.Dispose();
            Reader = null;

            Writer.Dispose();
            Writer = null;

            Error.Dispose();
            Error = null;

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
