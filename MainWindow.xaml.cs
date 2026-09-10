using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Just_A_Black_Screen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Hotkey ID: 9000 <-- [Ctrl + B]
        private const int HOTKEY_ID = 9000;

        //Modifier key: Ctrl = 0x0002
        private const uint MOD_CONTROL = 0x0002;

        //Key B = 0x42
        private const uint VK_B = 0x42;

        //Windows global hotkey code (786)
        private const int WM_HOTKEY = 0x0312;

        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private System.Windows.Forms.NotifyIcon? _trayIcon;

        public MainWindow()
        {
            InitializeComponent();

            //Forces Windows to assign an HWD even if hidden
            new WindowInteropHelper(this).EnsureHandle();

            ConfigTray();

        }

        private void ConfigTray()
        {
            var menu = new System.Windows.Forms.ContextMenuStrip();

            menu.Items.Add("Exit", null, (s, e) =>
            {
                //"Exit" button to close the app
                System.Windows.Application.Current.Shutdown();
            });

            //Icon
            _trayIcon = new System.Windows.Forms.NotifyIcon
            {
                //For the moment, just the generic app icon
                Icon = System.Drawing.SystemIcons.Application,
                Visible = true,
                Text = "Just A Black Screen (Ctrl + B)",
                ContextMenuStrip = menu
            };
        }


        //This code asks Windows to be aware of our Hotkey
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            //Takes HWND of the window
            IntPtr handle = new WindowInteropHelper(this).Handle;

            //Windows message queue
            HwndSource source = HwndSource.FromHwnd(handle);
            source.AddHook(WndProc);

            RegisterHotKey(handle, HOTKEY_ID, MOD_CONTROL, VK_B);
        }

        //Logic of the on/off toggle 
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //Is the message that arrived a global hotkey?
            if(msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
            {
                if (this.IsVisible)
                {
                    this.Hide();
                }
                else
                {
                    this.Show();
                    this.Activate();
                }

                handled = true;
            }

            return IntPtr.Zero;
        }

        //Code to free the HotKey when app is terminated
        protected override void OnClosed(EventArgs e)
        {
            if(_trayIcon != null)
            {
                _trayIcon.Visible = false;
                _trayIcon.Dispose();
            }

            IntPtr handled = new WindowInteropHelper(this).Handle;
            UnregisterHotKey(handled, HOTKEY_ID);
            base.OnClosed(e);
        }

        

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
                                                    // We use "System.Windows.Input... to prevent CS0104
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}