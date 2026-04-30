using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Pop_UP
{
    internal class download : Form
    {
        ProgressBar pbProgress = new ProgressBar();
        Timer timer = new Timer();
        int progress = 0;
        public download() 
        {
            int thisHeight = 400;
            int thisWidth = 550;
            this.Size = new Size(thisWidth, thisHeight);
            this.BackgroundImage = Image.FromFile("lege_tablet.png");

            pbProgress.Size = new Size(thisWidth - 100, thisHeight - 300);
            pbProgress.Location = new Point(50, 50);
            pbProgress.Minimum = 0;
            pbProgress.Maximum = 100;

            timer.Interval = 1000;
            timer.Tick += (tick, e) =>
            {
                if(progress >= 100)
                {
                    timer.Stop();
                    MessageBox.Show("Gewonnen! 🎉");
                    Form1.totalMs += 1500;
                    this.Close();
                    return;
                }
                progress += 5;
                pbProgress.Value = progress;
                //update_progress(progress);
            };

            Button start_btn = new Button();
            start_btn.Text = "Start download";
            start_btn.Location = new Point((thisWidth / 2) - (start_btn.Width / 2), pbProgress.Bottom + 20);
            start_btn.Click += (click, e) =>
            {
                timer.Start();
            };

            Controls.Add(start_btn);
            Controls.Add(pbProgress);
        }
    }
}
