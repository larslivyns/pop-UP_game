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
        private Dictionary<Color, List<PictureBox>> allePaden = new Dictionary<Color, List<PictureBox>>();
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
                //puzzel zeg maar al oplossen voor getoond word -> puzzel oplosbaar
                var roodVakjes = gekleurdeVakjes.Where(k => k.Value == Color.Red).Select(k => k.Key).ToList();
                var blauwVakjes = gekleurdeVakjes.Where(k => k.Value == Color.Blue).Select(k => k.Key).ToList();
                var groenVakjes = gekleurdeVakjes.Where(k => k.Value == Color.Green).Select(k => k.Key).ToList();

                geldigBord = IsPuzzelOplosbaar(gekleurdeVakjes);
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
                        Console.WriteLine(vakje);
                        grid_pb.BackColor = gekleurdeVakjes[vakje];
                        grid_pb.Tag = "eindpunt";   
                    }
                }
                grid_pb.MouseDown += Grid_MouseDown;
                grid_pb.MouseEnter += Grid_MouseEnter;

                Controls.Add(grid_pb);
            }
        }
        /*private bool HeeftPad(int start, int eind, Dictionary<int, Color> geblokkeerd)
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
        }*/
        //start en end point moetn hetzelfde zn -> YAY
        private void Grid_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox geklikt = (PictureBox)sender;
            if (geklikt.BackColor == Color.LightGray) return;

            Color kleur = geklikt.BackColor;

            // Klik op een vakje van een bestaand pad -> wis heel dat pad
            if (allePaden.ContainsKey(kleur))
            {
                foreach (PictureBox pb in allePaden[kleur])
                    if (pb.Tag == null) pb.BackColor = Color.LightGray;
                allePaden.Remove(kleur);
                verbonden[kleur] = false;
            }

            // Start nieuw pad vanaf eindpunt
            if (geklikt.Tag != null)
            {
                startPb = geklikt;
                pbKleur = kleur;
                isMoving = true;
                pad.Clear();
                pad.Add(geklikt);
            }
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
                allePaden[pbKleur] = new List<PictureBox>(pad); // NIEUW: pad opslaan
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
        private bool IsPuzzelOplosbaar(Dictionary<int, Color> gekleurdeVakjes)
        {
            // Bouw per kleur de twee eindpunten op
            var kleurParen = gekleurdeVakjes
                .GroupBy(k => k.Value)
                .ToDictionary(g => g.Key, g => g.Select(k => k.Key).ToList());

            // Grid status: -1 = leeg, anders = bezet door kleur (als index)
            int[] grid = new int[25];
            for (int i = 0; i < 25; i++) grid[i] = -1;

            // Zet de eindpunten vast
            foreach (var kv in gekleurdeVakjes)
                grid[kv.Key] = kv.Value.ToArgb();

            var kleuren = kleurParen.Keys.ToList();
            return Backtrack(grid, kleurParen, kleuren, 0);
        }
        private bool Backtrack(int[] grid, Dictionary<Color, List<int>> kleurParen, List<Color> kleuren, int kleurIndex)
        {
            if (kleurIndex == kleuren.Count) return true; // alle kleuren verbonden

            Color kleur = kleuren[kleurIndex];
            int start = kleurParen[kleur][0];
            int eind = kleurParen[kleur][1];

            return ZoekPad(grid, start, eind, kleur.ToArgb(), kleurParen, kleuren, kleurIndex);
        }
        private bool ZoekPad(int[] grid, int huidig, int eind, int kleurArgb,Dictionary<Color, List<int>> kleurParen, List<Color> kleuren, int kleurIndex)
        {
            if (huidig == eind)
                return Backtrack(grid, kleurParen, kleuren, kleurIndex + 1);

            int[] buren = {
        huidig - 5,
        huidig + 5,
        huidig % 5 > 0 ? huidig - 1 : -1,
        huidig % 5 < 4 ? huidig + 1 : -1
    };

            foreach (int buur in buren)
            {
                if (buur < 0 || buur > 24) continue;
                if (grid[buur] != -1 && buur != eind) continue; // bezet (en niet het eindpunt)

                int oud = grid[buur];
                grid[buur] = kleurArgb;

                if (ZoekPad(grid, buur, eind, kleurArgb, kleurParen, kleuren, kleurIndex))
                    return true;

                grid[buur] = oud; // backtrack
            }

            return false;
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