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
        private Dictionary<Color, bool> verbonden = new Dictionary<Color, bool>();
        private int aantalKleuren;
        public Label lbl = new Label();

        private PictureBox startPb = null;
        private bool isMoving = false;
        private Color pbKleur;
        private List<PictureBox> pad = new List<PictureBox>();

        public puzzel_connect(string naam, Label lbl)
        {
            this.Size = new Size(266, 290);
            this.MouseUp += Grid_MouseUp;

            this.lbl = lbl;

            //rnd een aantal vakjes vulln me kleurkes
            Dictionary<int, Color> gekleurdeVakjes = new Dictionary<int, Color>();
            /*gekleurdeVakjes.Add(Color.Red, rnd.Next(0, 25));
            gekleurdeVakjes.Add(Color.Blue, rnd.Next(0, 25));
            gekleurdeVakjes.Add(Color.Yellow, rnd.Next(0, 25));*/
            //ff checken
            Color[] kleuren = { Color.Red, Color.Blue, Color.Green, Color.Red, Color.Blue, Color.Green };
            aantalKleuren = kleuren.Distinct().Count();
            List<int> gebruikteVakjes = new List<int>();
            // Vervang de huidige loop in de constructor door dit:
            bool geldigBord = false;
            while (!geldigBord)
            {
                gekleurdeVakjes.Clear();
                gebruikteVakjes.Clear();

                for (int i = 0; i < 6; i++)
                {
                    int randomVakje;
                    do { randomVakje = rnd.Next(0, 25); }
                    while (gebruikteVakjes.Contains(randomVakje));
                    gebruikteVakjes.Add(randomVakje);
                    gekleurdeVakjes.Add(randomVakje, kleuren[i]);
                }

                // Check of rood->rood en blauw->blauw een pad hebben
                var roodVakjes = gekleurdeVakjes.Where(k => k.Value == Color.Red).Select(k => k.Key).ToList();
                var blauwVakjes = gekleurdeVakjes.Where(k => k.Value == Color.Blue).Select(k => k.Key).ToList();
                var groenVakjes = gekleurdeVakjes.Where(k => k.Value == Color.Green).Select(k => k.Key).ToList();

                bool roodOk = HeeftPad(roodVakjes[0], roodVakjes[1], gekleurdeVakjes);
                bool blauwOk = HeeftPad(blauwVakjes[0], blauwVakjes[1], gekleurdeVakjes);
                bool groenOk = HeeftPad(groenVakjes[0], groenVakjes[1], gekleurdeVakjes);

                geldigBord = roodOk && blauwOk && groenOk;
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
        private bool HeeftPad(int start, int eind, Dictionary<int, Color> geblokkeerd)
        {
            Color startKleur = geblokkeerd[start];
            Queue<int> queue = new Queue<int>();
            HashSet<int> bezocht = new HashSet<int>();
            queue.Enqueue(start);
            bezocht.Add(start);

            while (queue.Count > 0)
            {
                int huidig = queue.Dequeue();
                if (huidig == eind) return true;

                int[] buren = {
            huidig - 5, // boven
            huidig + 5, // onder
            huidig % 5 > 0 ? huidig - 1 : -1, // links
            huidig % 5 < 4 ? huidig + 1 : -1  // rechts
        };

                foreach (int buur in buren)
                {
                    if (buur < 0 || buur > 24) continue;
                    if (bezocht.Contains(buur)) continue;
                    // Mag er door als het leeg is OF het eindpunt
                    if (!geblokkeerd.ContainsKey(buur) || buur == eind)
                    {
                        bezocht.Add(buur);
                        queue.Enqueue(buur);
                    }
                }
            }
            return false;
        }
        //start en end point moetn hetzelfde zn -> YAY
        private void Grid_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox geklikt = (PictureBox)sender;
            if (geklikt.BackColor == Color.LightGray) return;
            startPb = geklikt;
            pbKleur = geklikt.BackColor;
            isMoving = true;
            verbonden[pbKleur] = false; // reset
            pad.Clear();
            pad.Add(geklikt);
        }
        private void Grid_MouseEnter(object sender, EventArgs e)
        {
            if (!isMoving) return;
            PictureBox hovered = (PictureBox)sender;

            // Teruggaan -> wis het stuk na dit vakje
            if (pad.Contains(hovered))
            {
                int index = pad.IndexOf(hovered);
                for (int i = pad.Count - 1; i > index; i--)
                {
                    pad[i].BackColor = Color.LightGray;
                    pad.RemoveAt(i);
                }
                return;
            }

            if (hovered.BackColor == pbKleur && hovered != startPb)
            {
                pad.Add(hovered);
                isMoving = false;
                verbonden[pbKleur] = true;
                CheckWin();
                return;
            }

            if (hovered.BackColor == Color.LightGray)
            {
                hovered.BackColor = pbKleur;
                pad.Add(hovered);
            }
            else
            {
                foreach (PictureBox pb in pad)
                    if (pb != startPb) pb.BackColor = Color.LightGray;
                pad.Clear();
                isMoving = false;
            }
        }

        private void Grid_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isMoving && pad.Count > 0 && pad.Last().BackColor == pbKleur)
            { 
                startPb = null;
                pad.Clear();
                return;
            }

            if (isMoving)
            {
                foreach (PictureBox pb in pad)
                    if (pb != startPb) pb.BackColor = Color.LightGray;
            }

            startPb = null;
            isMoving = false;
            pad.Clear();
        }
        private void CheckWin()
        {
            if (verbonden.Count == aantalKleuren && verbonden.Values.All(v => v))
            {
                MessageBox.Show("Gewonnen! 🎉");
                Form1.totalMs += 1500;
                this.Close();
            } 
        }
    }
}