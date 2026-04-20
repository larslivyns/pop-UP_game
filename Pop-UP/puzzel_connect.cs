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
        Random rnd = new Random();

        private PictureBox startPb = null;
        private bool isMoving = false;
        private Color pbKleur;
        private List<PictureBox> pad = new List<PictureBox>();

        public puzzel_connect(string naam)
        {
            this.Size = new Size(266, 290);
            this.MouseUp += Grid_MouseUp;

            //rnd een aantal vakjes vulln me kleurkes
            Dictionary<int, Color> gekleurdeVakjes = new Dictionary<int, Color>();
            /*gekleurdeVakjes.Add(Color.Red, rnd.Next(0, 25));
            gekleurdeVakjes.Add(Color.Blue, rnd.Next(0, 25));
            gekleurdeVakjes.Add(Color.Yellow, rnd.Next(0, 25));*/
            //ff checken
            Color[] kleuren = { Color.Red, Color.Blue, Color.Red, Color.Blue };
            List<int> gebruikteVakjes = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                int randomVakje;
                do //online gevonden, blijft een nieuw getal maken totdat het nie bestaat in de list
                { randomVakje = rnd.Next(0, 25); }
                while (gebruikteVakjes.Contains(randomVakje));
                gebruikteVakjes.Add(randomVakje);
                gekleurdeVakjes.Add(randomVakje, kleuren[i]);
                Console.WriteLine(gekleurdeVakjes[randomVakje]);
            }
            //speelveld opzettn
            for (int i = 0; i < 25; i++)
            {
                PictureBox grid_pb = new PictureBox();
                grid_pb.Size = new Size(50, 50);
                grid_pb.Location = new Point((i % 5) * 50, (i / 5) * 50);
                grid_pb.BorderStyle = BorderStyle.FixedSingle;
                grid_pb.BackColor = Color.LightGray;
                foreach (int vakje in gebruikteVakjes)
                {
                    if (i == vakje)
                    {
                        Console.WriteLine(vakje); //nummers
                        grid_pb.BackColor = gekleurdeVakjes[vakje];
                    }
                }
                grid_pb.MouseDown += Grid_MouseDown;
                grid_pb.MouseEnter += Grid_MouseEnter;

                Controls.Add(grid_pb);
            }
        }
        //start en end point moetn hetzelfde zn -> YAY
        private void Grid_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox geklikt = (PictureBox)sender;
            if (geklikt.BackColor == Color.LightGray) return;

            startPb = geklikt;
            pbKleur = geklikt.BackColor;
            isMoving = true;
            pad.Clear();
            pad.Add(geklikt);
        }
        private void Grid_MouseEnter(object sender, EventArgs e)
        {
            if (!isMoving) return;
            PictureBox hovered = (PictureBox)sender;
            if (pad.Contains(hovered)) return;
            if (hovered.BackColor == pbKleur) return; // eindpunt, niet inkleuren

            if (hovered.BackColor == Color.LightGray)
            {
                hovered.BackColor = pbKleur;
                pad.Add(hovered);
            }
        }
        private void Grid_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isMoving || startPb == null) return;

            // Kijk welke cel onder de muis zit
            Point muisPositie = e.Location;
            PictureBox losgelaten = null;

            foreach (Control c in Controls)
            {
                if (c is PictureBox pb && pb.Bounds.Contains(muisPositie))
                {
                    losgelaten = pb;
                    break;
                }
            }
            if (losgelaten != null)
            {
                bool succes = losgelaten != startPb && losgelaten.BackColor == pbKleur;
                if (!succes)
                {
                    foreach (PictureBox pb in pad)
                        if (pb != startPb) pb.BackColor = Color.LightGray;
                }
            }
            startPb = null;
            isMoving = false;
            pad.Clear();
        }
    }
}