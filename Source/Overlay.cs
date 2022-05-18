public class Overlay
    {
        public string _WindowName;

        public bool _IsLooping = true;

        public static IntPtr _WindowHandle;

        public Overlay(string WindowName) => _WindowName = WindowName;

        public void SetupOverlay(Form OverlayForm)
        {
            if (_WindowName == null || _WindowName == string.Empty)
            {
                MessageBox.Show("Error: Window was not setup correctly!", "Overlay Sharp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            Random random = new Random();
            OverlayForm.BackColor = Color.Wheat;
            OverlayForm.TransparencyKey = Color.Wheat;
            OverlayForm.TopMost = true;
            OverlayForm.Text = RandomString(random.Next(15, 30));
            OverlayForm.ShowIcon = false;

            int initialStyle = GetWindowLong(OverlayForm.Handle, -20);
            SetWindowLong(OverlayForm.Handle, -20, initialStyle | 0x8000 | 0x20);
        }
        
        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void LoopOverlay(Form OverlayForm, int Interval)
        {
            if (Interval < 1 || Interval > 150)
            {
                MessageBox.Show("Error: The interval was set to low or high! Make sure its between 1-150...", "Overlay Sharp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            System.Threading.Thread LoopThread = new System.Threading.Thread(() => Loop(OverlayForm, Interval)) { IsBackground = true };
            LoopThread.Start();
        }

        private void Loop(Form OverlayForm, int Interval)
        {
            while (true)
            {
                if (_IsLooping)
                {
                    _WindowHandle = FindWindow(null, _WindowName);
                    GetWindowRect(_WindowHandle, out rect);
                    OverlayForm.Size = MagicSize();
                    OverlayForm.Left = rect.left;
                    OverlayForm.Top = rect.top;
                }

                System.Threading.Thread.Sleep(Interval);
            }
        }

        private Size MagicSize()
        {
            Size MAGIC = new Size(rect.right - rect.left, rect.bottom - rect.top);
            return MAGIC;
        }

        public void SwitchLoop()
        {
            _IsLooping =! _IsLooping;
        }

        public class Render
        {
            public static void AddCircle(Panel RenderPanel, Graphics RenderGraphics, Pen RenderColor, int RenderSize)
            {
                RenderGraphics.DrawEllipse(RenderColor, new Rectangle((RenderPanel.ClientSize.Width / 2) - (RenderSize / 2), (RenderPanel.ClientSize.Height / 2) - (RenderSize / 2), RenderSize, RenderSize));
            }
        }
    }
