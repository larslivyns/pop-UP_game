using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pop_UP
{
    internal class puzzel_1 : Form1
    {
        public string naam;

        public event EventHandler Ready;

        public puzzel_1()
        {
            //kijk omda wa aantepassn naar een puzzel toe

            TextBox textBox = new TextBox();
            textBox.Size = new Size(100, 30);
            textBox.Location = new Point(10, 10);
            Controls.Add(textBox);
            textBox.Text = naam;

            Button button = new Button();
            button.Size = new Size(100, 30);
            button.Location = new Point(120, 10);
            button.Text = "Bewaar";
            Controls.Add(button);

            button.Click += (sender, e) =>
            {
                this.naam = textBox.Text;
                Ready?.Invoke(this, EventArgs.Empty);
            };
        }
}
}
