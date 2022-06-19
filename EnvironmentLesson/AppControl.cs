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
    public partial class AppControl : UserControl
    {
        public int XAxis;
        public int YAxis;
        public AppControl()
        {
            InitializeComponent();
            this.panel1.Size = new Size(32, 32);
            this.XAxis = 1;
            this.YAxis = 1;
            this.label1.MaximumSize = new Size(64, 100);
            this.label1.Location = new Point(this.label1.Location.X, this.label1.Location.Y + 15);
            //this.label1.BackColor = Color.Transparent;
            // this.label1.Parent.BackColor = Color.Transparent;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.MakeTransparentControls(this);
            this.DoubleBuffered = true;
            this.Click += DeleteByClickEvent;
        }

        private void DeleteByClickEvent(object sender, EventArgs e)
        {
           // Form1.deleted.Add(sender as AppControl);
            Form1.buttons.Remove(sender as AppControl);
            this.Dispose();
        }

        /* protected override void OnPaintBackground(PaintEventArgs e)
{
    return;
}*/
        private void MakeTransparentControls(Control parent)
        {
            if (parent.Controls != null && parent.Controls.Count > 0)
            {
                foreach (Control control in parent.Controls)
                {
                    if ((control is PictureBox) || (control is Label) || (control is GroupBox) || (control is CheckBox))
                        control.BackColor = Color.Transparent;

                    if (control.Controls != null && control.Controls.Count > 0)
                        MakeTransparentControls(control);
                }
            }
        }
        private void AppControl_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
