using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Zeus_v01
{
    public partial class Form1 : Form
    { 


        ListViewItem lv;
        public int a, b;
        public static int click;

        private IntPtr appWin;
        static readonly int GWL_STYLE = -16;
        static readonly int WS_VISIBLE = 0x10000000;
        private Process p = null;
        private Process[] pnew = null;

        Scripts scripts;

       
        MouseTracker mouseTracker = new MouseTracker();

        // Mouse click parameters
        private const UInt32 LEFTDOWN = 0x0002;
        private const UInt32 LEFTUP = 0x0004;
        private const UInt32 RIGHTUP = 0x0010;
        private const UInt32 RIGHTDOWN = 0x0008;
        private const UInt32 MIDDLEDOWN = 0x0020;
        private const UInt32 MIDDLEUP = 0x0040;

        private const int BM_CLICK = 0x00F5;

        Random r = new Random();

        public Process pr = new Process();

        public string userString = "USERNAME";
        public string passwordString = "PASSWORD";

        // Mouse Event Import
        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInf);
        [DllImport("user32.dll")]
        // Cursor position
        private static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        static extern bool MoveWindow(IntPtr Handle, int x, int y, int w, int h, bool repaint);

        public Form1() { InitializeComponent(); scripts = new Scripts(); }     

        private void OpenGame()
        {
            try
            {
                pr.StartInfo.FileName = "C:/ProgramData/Jagex/launcher/rs2client.exe";
                pr.StartInfo.CreateNoWindow = true;
                pr.Start();
                Thread.Sleep(8000);
                pnew = Process.GetProcessesByName("rs2client");
                p = pnew[0];
                p.WaitForInputIdle();
                appWin = p.MainWindowHandle; // Get the main handle
                SetParent(p.MainWindowHandle, panel1.Handle); // Set the panel parent
                SetParent(appWin, panel1.Handle); // Put it into this form
                SetWindowLong(appWin, GWL_STYLE, WS_VISIBLE); // Remove border and whatnot
                MoveWindow(appWin, 0, 0, panel1.Width, panel1.Height, true); // Move the window to overlay it on this window
            }
            catch (Exception ex)
            {
                MessageBox.Show(null, ex.Message, "Error");
            }
        }

        // Prevent movement of window
        protected override void WndProc(ref Message message)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;

            switch (message.Msg)
            {
                case WM_SYSCOMMAND:
                    int command = message.WParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                        return;
                    break;
            }
            base.WndProc(ref message);
        }

        private void loadRunescapeToolStripMenuItem_Click(object sender, EventArgs e) { OpenGame(); } // On Press, Load Runescape into Screen

        private void archeologyV01ToolStripMenuItem_Click(object sender, EventArgs e) { scripts.ArcheologyScriptV1();  }

        private void Form1_Load(object sender, EventArgs e) => a = b = click = 0;
        
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            a = 0;
            b = 0;
            click = 0;
        }

        private void autoLoginToolStripMenuItem_Click(object sender, EventArgs e) => scripts.AutoLogin();

        private void recordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mouseTracker.Subscribe(Hook.GlobalEvents());
            timer1.Start();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mouseTracker.Unsubscribe();
            timer1.Stop();
            timer2.Stop();
            timer1.Dispose();
            timer2.Dispose();
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e) => timer2.Start();

        private void killRunescapeToolStripMenuItem_Click(object sender, EventArgs e) => scripts.killRS();

        private void button1_Click(object sender, EventArgs e)
        {
            userString = textBox1.Text;
            passwordString = textBox2.Text;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lv = new ListViewItem(Cursor.Position.X.ToString());
            lv.SubItems.Add(Cursor.Position.Y.ToString());
            lv.SubItems.Add(click.ToString());
            listView1.Items.Add(lv);
            Console.WriteLine(click);
            b++;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (a != b)
            {
                Cursor.Position = new Point(int.Parse(listView1.Items[a].SubItems[0].Text),
                    int.Parse(listView1.Items[a].SubItems[1].Text));
                int newclick = int.Parse(listView1.Items[a].SubItems[2].Text);
                switch (newclick)
                {
                    case 1:
                        LeftClickMouse();
                        break;
                    case 2:
                        RightClickMouse();
                        break;
                    case 3:
                        MiddleClickMouse();
                        break;
                }
                a++;
            }
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {         // On Press of Red X
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.WindowsShutDown) return;
            // Confirm user wants to close
            switch (MessageBox.Show(this, "Shutting down Zeus", "Press Yes To Confirm", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                default:
                    Thread.Sleep(300);
                    scripts.killRS();
                    break;
            }
        }

        public void DoubleClickAtPosition(int posX, int posY)
        {
            
            SetCursorPos(posX, posY);
            LeftClickMouse();
            Thread.Sleep(r.Next(250, 350));
            LeftClickMouse();
        }

        // Add mouse offset from panel 0, 0
        public void setMousePos(int x, int y)
        {
            x += 411;
            y += 253;
            SetCursorPos(x, y);
        }

        public void LeftClickMouse()
        {
            Thread.Sleep(r.Next(30, 70));
            mouse_event(LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(r.Next(254, 700));
            mouse_event(LEFTUP, 0, 0, 0, 0);
        }

        private void RightClickMouse()
        {
            mouse_event(RIGHTDOWN, 0, 0, 0, 0);
            mouse_event(RIGHTUP, 0, 0, 0, 0);
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (DirtCheckBox.Checked)
            {
                scripts.dropthedirt = true;
            }
            else
            {
                scripts.dropthedirt = false;
            }
        }

        private void WorldCheckBox_Click(object sender, EventArgs e)
        {
            if (WorldCheckBox.Checked)
            {
                scripts.selectWorld = true;
            }
            else
            {
                scripts.selectWorld = false;
            }
        }

        private void button2_Click(object sender, EventArgs e) => scripts.abortThread();

        private void button3_Click(object sender, EventArgs e)
        {
            Thread t2 = new Thread(() =>
            {
                scripts.inventoryCount();
            });
            t2.Start();
        }

        private void MiddleClickMouse()
        {
            mouse_event(MIDDLEDOWN, 0, 0, 0, 0);
            mouse_event(MIDDLEUP, 0, 0, 0, 0);
        }
    }
}