using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Pop_UP
{
    internal class pop_up1 : Form
    {
        Random rnd = new Random();
       public pop_up1()
        {
            int thisHeight = rnd.Next(100, 300);
            int thisWidth = rnd.Next(100, 300);
            this.Size = new Size(thisWidth, thisHeight);

            Button btn = new Button();
            btn.Text = "Close";
            btn.Size = new Size(50,50);
            int btnWidth = rnd.Next(0, thisWidth - btn.Width);
            int btnHeight = rnd.Next(0, thisHeight - btn.Height);
            btn.Location = new Point(btnWidth, btnHeight);
            Controls.Add(btn);

            btn.Click += (click, e) =>
            {
                this.Close();
            };
        }

    }
}
