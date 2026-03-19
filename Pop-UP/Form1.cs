using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pop_UP
{
    public partial class Form1 : Form
    {
        int pb_width = 600;
        int pb_height = 600;
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(pb_width, pb_height);

            //eerst het startscherm
            PictureBox pb_main = new PictureBox();
            pb_main.Size = new Size(pb_width, pb_height);
            
            Label lbl_titel = new Label();
            lbl_titel.Location = new Point(pb_width / 2 - 100, pb_height / 2 - 90);
            lbl_titel.Size = new Size(200, 60);
            lbl_titel.Text = "POP-UP";
            lbl_titel.TextAlign = ContentAlignment.MiddleCenter;
            lbl_titel.Font = new Font("Bebas Neue", 24f, FontStyle.Regular);
            
            Button btn_start = new Button();
            btn_start.Location = new Point(pb_width / 2 - 100, pb_height/2 - 30);
            btn_start.Size = new Size(200, 60);
            btn_start.Text = "start game";

            //alle controls add
            Controls.Add(pb_main);
            pb_main.Controls.Add(lbl_titel);
            pb_main.Controls.Add(btn_start);
        }
    }
}
