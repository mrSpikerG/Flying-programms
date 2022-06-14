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
            this.label1.MaximumSize = new Size(64,100);
            this.label1.Location = new Point(this.label1.Location.X, this.label1.Location.Y + 15);
        }

        private void AppControl_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
