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
        Timer timer = new Timer();
        public static int totalMs = 10 * 1000;
        public Label lbl_titel = new Label();
        Random rnd = new Random();
        bool game_started = false;
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(pb_width, pb_height);

            //eerst het startscherm
            PictureBox pb_main = new PictureBox();
            pb_main.Size = new Size(pb_width, pb_height);
           
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

            //kleine test  
            btn_start.Click += (sender, e) =>
            {
                game_started = true;
                puzzel_connect puzzel_connect = new puzzel_connect("testje", lbl_titel);
                puzzel_connect.ShowDialog();
                timer.Start();
                btn_start.Text = "Volgende Puzzel";
            };

            
            lbl_titel.Text = "10:000";
            timer.Interval = 1;
            List<pop_up1> pop_ups = new List<pop_up1>();
            timer.Tick += (sender2, e2) =>
            {
                
                totalMs--;
                int seconden = totalMs / 1000;
                int ms = totalMs % 1000;
                lbl_titel.Text = $"{seconden:D2}:{ms:D3}";
                if (totalMs <= 0)
                {
                    timer.Stop();
                    lbl_titel.Text = "00:000";
                }
                int add = rnd.Next(0, 300);
                if (add == 1 && game_started && pop_ups.Count < 5)
                {
                    pop_up1 pop_Up1 = new pop_up1();
                    pop_Up1.ShowDialog();
                    pop_ups.Add(pop_Up1);
                }
            };
            //timer.Start();
        }
    }
}
