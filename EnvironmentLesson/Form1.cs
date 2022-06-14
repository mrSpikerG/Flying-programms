using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentLesson
{
    public partial class Form1 : Form
    {
        Random rand = new Random();
        List<AppControl> buttons = new List<AppControl>();
        List<string> names = new List<string>();
        List<string> icons = new List<string>();
        Timer timer;
        public Form1()
        {
            InitializeComponent();

            //
            //  Main Settings
            //
            this.StartPosition = FormStartPosition.Manual;
            this.WindowState = FormWindowState.Maximized;


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
            timer = new Timer();
            timer.Tick += moveIcons;
            timer.Interval = 100;

            //
            //  Main Button Event
            //
            this.button1.Click += button1_Click;
        }

        private void moveIcons(object sender, EventArgs e)
        {
            for (int i = 0; i < this.buttons.Count; i++)
            {
                //
                // To many if :<
                //
                this.buttons[i].Location = new Point(this.buttons[i].Location.X + getXAxis(i),
                    this.buttons[i].Location.Y + getYAxis(i));
            }
        }

        private int getXAxis(int id)
        {
            if (this.buttons[id].Location.X < 0)
            {
                this.buttons[id].XAxis = -this.buttons[id].XAxis;
                //return 1;
            }
            if (this.buttons[id].Location.X + this.buttons[id].Size.Width > this.ClientSize.Width)
            {
                this.buttons[id].XAxis = -this.buttons[id].XAxis;
                //return -1;
            }
            return this.buttons[id].XAxis;
            //return rand.Next(-1, 2);
        }
        private int getYAxis(int id)
        {
            if (this.buttons[id].Location.Y < 0)
            {
                this.buttons[id].YAxis = -this.buttons[id].YAxis;
                //return 1;
            }
            if (this.buttons[id].Location.Y + this.buttons[id].Size.Height > this.ClientSize.Height)
            {
                this.buttons[id].YAxis = -this.buttons[id].YAxis;
                //return -1;
            }
            return this.buttons[id].YAxis;
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
                    this.buttons.Add(TEMP);
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
                    this.buttons.Add(TEMP);
                }
            }

            for (int i = 0; i < this.buttons.Count; i++)
            {
                this.Controls.Add(this.buttons[i]);
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
    }
}
