using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pop_UP
{
    internal class puzzel_connect : Form1
    {
        public string naam;

        public event EventHandler Ready;
        public puzzel_connect(string naam)
        {
            Label lbl_test = new Label();
            lbl_test.Text = naam;
            Controls.Add(lbl_test);
        }
    }
}
