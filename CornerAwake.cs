using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace CornerAwake
{
    public class Program
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint SetThreadExecutionState(uint esFlags);

        private const uint ES_CONTINUOUS = 0x80000000;
        private const uint ES_SYSTEM_REQUIRED = 0x00000001;
        private const uint ES_DISPLAY_REQUIRED = 0x00000002;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        private static System.Threading.Mutex singleMutex;

        [STAThread]
        public static void Main()
        {
            bool isFirst;
            singleMutex = new System.Threading.Mutex(true, "CornerAwake_SingleInstance_App", out isFirst);

            if (!isFirst)
            {
                MessageBox.Show(
                    "CornerAwake est déjà actif dans la barre des tâches (près de l'horloge).",
                    "CornerAwake",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public class MainForm : Form
        {
            private NotifyIcon trayIcon;
            private Timer checkTimer;
            private bool isAwakeActive = false;
            private int cornerThreshold = 35; // Pixels depuis le bord haut et gauche

            private Icon iconIdle;
            private Icon iconActive;

            private MenuItem statusItem;
            private MenuItem startupItem;

            private const string RUN_REG_KEY = @"Software\Microsoft\Windows\CurrentVersion\Run";
            private const string APP_NAME = "CornerAwake";

            public MainForm()
            {
                // Fenêtre invisible en dehors de l'écran pour maintenir la boucle Windows Forms
                this.FormBorderStyle = FormBorderStyle.None;
                this.ShowInTaskbar = false;
                this.Size = new Size(0, 0);
                this.Location = new Point(-20000, -20000);
                this.Opacity = 0;

                try
                {
                    iconIdle = BuildIcon(false);
                    iconActive = BuildIcon(true);

                    var menu = new ContextMenu();
                    statusItem = new MenuItem("Statut : 💤 Veille normale") { Enabled = false };
                    menu.MenuItems.Add(statusItem);
                    menu.MenuItems.Add("-");

                    var zoneMenu = new MenuItem("Taille de la zone");
                    var m20 = new MenuItem("Petite (20x20 px)", (s, e) => ChangeZoneSize(20, (MenuItem)s));
                    var m35 = new MenuItem("Moyenne (35x35 px - Conseillé)", (s, e) => ChangeZoneSize(35, (MenuItem)s)) { Checked = true };
                    var m50 = new MenuItem("Grande (50x50 px)", (s, e) => ChangeZoneSize(50, (MenuItem)s));
                    zoneMenu.MenuItems.Add(m20);
                    zoneMenu.MenuItems.Add(m35);
                    zoneMenu.MenuItems.Add(m50);
                    menu.MenuItems.Add(zoneMenu);

                    startupItem = new MenuItem("Démarrer avec Windows", OnToggleStartup);
                    startupItem.Checked = CheckStartup();
                    menu.MenuItems.Add(startupItem);

                    menu.MenuItems.Add("-");
                    menu.MenuItems.Add(new MenuItem("À propos", (s, e) =>
                    {
                        MessageBox.Show(
                            "CornerAwake ⚡\n\n" +
                            "• Déplacez votre souris tout en haut à gauche de l'écran pour empêcher la mise en veille de votre écran et de votre PC.\n" +
                            "• Retirez simplement la souris du coin pour que la mise en veille normale reprenne son cours habituel.\n\n" +
                            "Ultra-léger (~11 Ko, 0% CPU, ~5 Mo RAM) et 100% natif Windows.",
                            "À propos de CornerAwake",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }));

                    menu.MenuItems.Add(new MenuItem("Quitter", (s, e) => Quit()));

                    trayIcon = new NotifyIcon
                    {
                        Icon = iconIdle,
                        ContextMenu = menu,
                        Text = "CornerAwake : Veille normale 💤",
                        Visible = true
                    };

                    checkTimer = new Timer();
                    checkTimer.Interval = 300; // Vérification toutes les 300 ms (0.0% CPU)
                    checkTimer.Tick += OnTick;
                    checkTimer.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur au lancement de CornerAwake: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Quit();
                }
            }

            private void ChangeZoneSize(int size, MenuItem clicked)
            {
                cornerThreshold = size;
                foreach (MenuItem item in clicked.Parent.MenuItems)
                {
                    item.Checked = false;
                }
                clicked.Checked = true;
            }

            private void OnTick(object sender, EventArgs e)
            {
                try
                {
                    POINT pt;
                    if (!GetCursorPos(out pt)) return;

                    Rectangle screen = Screen.PrimaryScreen.Bounds;
                    bool inCorner = (pt.X >= screen.Left && pt.X <= screen.Left + cornerThreshold &&
                                     pt.Y >= screen.Top && pt.Y <= screen.Top + cornerThreshold);

                    if (inCorner)
                    {
                        if (!isAwakeActive)
                        {
                            SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);
                            isAwakeActive = true;
                            trayIcon.Icon = iconActive;
                            trayIcon.Text = "CornerAwake : ANTI-VEILLE ACTIF ⚡";
                            statusItem.Text = "Statut : ⚡ Anti-veille ACTIF";
                        }
                        else
                        {
                            // Renouvellement de sécurité pour la gestion d'énergie de Windows
                            SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);
                        }
                    }
                    else
                    {
                        if (isAwakeActive)
                        {
                            ResetSleep();
                            isAwakeActive = false;
                            trayIcon.Icon = iconIdle;
                            trayIcon.Text = "CornerAwake : Veille normale 💤";
                            statusItem.Text = "Statut : 💤 Veille normale";
                        }
                    }
                }
                catch { }
            }

            private void ResetSleep()
            {
                try
                {
                    SetThreadExecutionState(ES_CONTINUOUS);
                }
                catch { }
            }

            private bool CheckStartup()
            {
                try
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_REG_KEY, false))
                    {
                        return key != null && key.GetValue(APP_NAME) != null;
                    }
                }
                catch { return false; }
            }

            private void OnToggleStartup(object sender, EventArgs e)
            {
                try
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_REG_KEY, true))
                    {
                        if (key != null)
                        {
                            if (startupItem.Checked)
                            {
                                key.DeleteValue(APP_NAME, false);
                                startupItem.Checked = false;
                            }
                            else
                            {
                                key.SetValue(APP_NAME, "\"" + Application.ExecutablePath + "\"");
                                startupItem.Checked = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Impossible de changer le démarrage automatique : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            private void Quit()
            {
                ResetSleep();
                if (checkTimer != null) checkTimer.Stop();
                if (trayIcon != null)
                {
                    trayIcon.Visible = false;
                    trayIcon.Dispose();
                }
                this.Close();
                Application.Exit();
            }

            protected override void OnFormClosing(FormClosingEventArgs e)
            {
                ResetSleep();
                base.OnFormClosing(e);
            }

            private Icon BuildIcon(bool active)
            {
                Bitmap bmp = new Bitmap(32, 32);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    if (active)
                    {
                        // Cercle vert émeraude vif
                        using (Brush b = new SolidBrush(Color.FromArgb(16, 185, 129)))
                        {
                            g.FillEllipse(b, 1, 1, 30, 30);
                        }
                        // Éclair blanc stylisé
                        using (Brush b = new SolidBrush(Color.White))
                        {
                            Point[] bolt = new Point[] {
                                new Point(17, 4),
                                new Point(9, 16),
                                new Point(16, 16),
                                new Point(14, 28),
                                new Point(24, 14),
                                new Point(17, 14)
                            };
                            g.FillPolygon(b, bolt);
                        }
                    }
                    else
                    {
                        // Cercle ardoise / bleu nuit
                        using (Brush b = new SolidBrush(Color.FromArgb(71, 85, 105)))
                        {
                            g.FillEllipse(b, 1, 1, 30, 30);
                        }
                        // Lune claire
                        using (Brush b = new SolidBrush(Color.FromArgb(226, 232, 240)))
                        {
                            g.FillEllipse(b, 7, 7, 18, 18);
                        }
                        using (Brush b = new SolidBrush(Color.FromArgb(71, 85, 105)))
                        {
                            g.FillEllipse(b, 11, 5, 16, 16);
                        }
                    }
                }

                return Icon.FromHandle(bmp.GetHicon());
            }
        }
    }
}
