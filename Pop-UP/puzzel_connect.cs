using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pop_UP
{
    internal class puzzel_connect : Form
    {
        public string naam;
        
        public puzzel_connect(string naam)
        {
            this.Size = new Size(266,290);
            
            //rnd een aantal vakjes vulln me kleurkes
            //speelveld opzettn
            for (int i = 0; i < 25; i++)
            {
                PictureBox grid_pb = new PictureBox();
                grid_pb.Size = new Size(50,50);
                grid_pb.Location = new Point((i % 5) * 50, (i / 5) * 50);
                grid_pb.BorderStyle = BorderStyle.FixedSingle;
                Controls.Add(grid_pb);

                Random rnd = new Random();
                int gekleurd_vierkant = rnd.Next(0, 3);
                Console.WriteLine(gekleurd_vierkant.ToString());
            }
        }
    }
}
