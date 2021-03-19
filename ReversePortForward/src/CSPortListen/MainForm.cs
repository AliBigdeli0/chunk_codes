using System;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace CSPortListen
{
    public partial class MainForm : Form
    {
        public static Action<string> SendMessage;
        bool _is_start = false;
        TcpListenSlim _tcp_slim;
        Thread _thread;
        public MainForm()
        {
            InitializeComponent();

            MainForm.SendMessage += SendMessageReplay; 
            _tcp_slim = new TcpListenSlim();
        }

        private void SendMessageReplay(string replaymessage)
        {
            textResult.BeginInvoke(new MethodInvoker(() => {
                textResult.Text += $"##{replaymessage}{Environment.NewLine}";
            }));
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textResult = new System.Windows.Forms.TextBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listenPortNumeric = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.listenPortNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // textResult
            // 
            this.textResult.Location = new System.Drawing.Point(12, 41);
            this.textResult.Multiline = true;
            this.textResult.Name = "textResult";
            this.textResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textResult.Size = new System.Drawing.Size(470, 308);
            this.textResult.TabIndex = 0;
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(325, 355);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(157, 23);
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "Ready";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Listen Port : ";
            // 
            // listenPortNumeric
            // 
            this.listenPortNumeric.Location = new System.Drawing.Point(72, 7);
            this.listenPortNumeric.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.listenPortNumeric.Name = "listenPortNumeric";
            this.listenPortNumeric.Size = new System.Drawing.Size(54, 20);
            this.listenPortNumeric.TabIndex = 3;
            this.listenPortNumeric.Value = new decimal(new int[] {
            8080,
            0,
            0,
            0});
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 391);
            this.Controls.Add(this.listenPortNumeric);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.textResult);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Connection Manager";
            ((System.ComponentModel.ISupportInitialize)(this.listenPortNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private void OkButton_Click(object sender, EventArgs e)
        {
            _is_start = !_is_start;
            try
            {
                if (_is_start)
                {
                    OkButton.Text = "Stop";
                    var port = (int)listenPortNumeric.Value;
                    _thread = new Thread(new ThreadStart(() => {
                        _tcp_slim.ListenStart(new IPEndPoint(IPAddress.Parse("0.0.0.0"), port));
                    }));
                    _thread.Start();
                    MainForm.SendMessage($"start listen on port: '{port}'");
                }
                else
                {
                    OkButton.Text = "Start";
                    _tcp_slim.ListenStop();
                    _thread.Abort();
                    _thread = null;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        void ShowError(string message)
        {
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
