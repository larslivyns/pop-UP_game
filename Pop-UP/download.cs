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
        public download() 
        {
            int thisHeight = 200;
            int thisWidth = 200;
            this.Size = new Size(thisWidth, thisHeight);
            PictureBox pbProgress = new PictureBox();
            pbProgress.Size = new Size(thisWidth - 50, thisHeight - 50);
        }
    }
}
