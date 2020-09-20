using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zeus_v01
{
    partial class MouseTracker : Form
    {
        private IKeyboardMouseEvents m_Events;

        public void Subscribe(IKeyboardMouseEvents events)
        {
            m_Events = events;
            m_Events.KeyDown += OnKeyDown;
            m_Events.KeyUp += OnKeyUp;
            m_Events.KeyPress += HookManager_KeyPress;
            m_Events.MouseUp += OnMouseUp;
            m_Events.MouseClick += OnMouseClick;
            m_Events.MouseDoubleClick += OnMouseDoubleClick;
            m_Events.MouseMove += HookManager_MouseMove;
            m_Events.MouseDragStarted += OnMouseDragStarted;
            m_Events.MouseDragFinished += OnMouseDragFinished;
            m_Events.MouseWheel += HookManager_MouseWheel;
            m_Events.MouseDown += OnMouseDown;
        }

        public void Unsubscribe()
        {
            if (m_Events == null) return;
            m_Events.KeyDown -= OnKeyDown;
            m_Events.KeyUp -= OnKeyUp;
            m_Events.KeyPress -= HookManager_KeyPress;
            m_Events.MouseUp -= OnMouseUp;
            m_Events.MouseClick -= OnMouseClick;
            m_Events.MouseDoubleClick -= OnMouseDoubleClick;
            m_Events.MouseMove -= HookManager_MouseMove;
            m_Events.MouseDragStarted -= OnMouseDragStarted;
            m_Events.MouseDragFinished -= OnMouseDragFinished;
            m_Events.MouseWheel -= HookManager_MouseWheel;
            m_Events.MouseDown -= OnMouseDown;
            m_Events.Dispose();
            m_Events = null;
        }

        private void Log(string text)
        {
            if (IsDisposed) return;
            //textBoxLog.AppendText(text);
            //textBoxLog.ScrollToCaret();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Log(string.Format("KeyDown  \t\t {0}\n", e.KeyCode));
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Log(string.Format("KeyUp  \t\t {0}\n", e.KeyCode));
        }

        private void HookManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            Log(string.Format("KeyPress \t\t {0}\n", e.KeyChar));
        }

        private void HookManager_MouseMove(object sender, MouseEventArgs e)
        {
            Console.WriteLine(string.Format("x={0:0000}; y={1:0000}", e.X, e.Y));
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine(string.Format("MouseDown \t\t {0}\n", e.Button));
            switch (e.Button.ToString().Trim())
            {
                case ("Left"):
                    Form1.click = 1;
                    break;
                case ("Right"):
                    Form1.click = 3;
                    break;
                case ("Middle"):
                    Form1.click = 5;
                    break;
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            
            Log(string.Format("MouseUp \t\t {0}\n", e.Button));
            Console.WriteLine(e.Button);
            switch (e.Button.ToString())
            {
                case ("Left"):
                    Form1.click = 0;
                    break;
                case ("Right"):
                    Form1.click = 2;
                    break;
                case ("Middle"):
                    Form1.click = 4;
                    break;
            }
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            Log(string.Format("MouseClick \t\t {0}\n", e.Button));
        }

        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            Log(string.Format("MouseDoubleClick \t\t {0}\n", e.Button));
        }

        private void OnMouseDragStarted(object sender, MouseEventArgs e)
        {
            Log("MouseDragStarted\n");
        }

        private void OnMouseDragFinished(object sender, MouseEventArgs e)
        {
            Log("MouseDragFinished\n");
        }

        private void HookManager_MouseWheel(object sender, MouseEventArgs e)
        {
           Console.WriteLine(string.Format("Wheel={0:000}", e.Delta));
        }

        private void HookManager_MouseWheelExt(object sender, MouseEventExtArgs e)
        {
            Console.WriteLine(string.Format("Wheel={0:000}", e.Delta));
            Log("Mouse Wheel Move Suppressed.\n");
            e.Handled = true;
        }
    }
}
