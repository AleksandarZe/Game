using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Media;

namespace Game
{
    public partial class frmGame : Form
    {
        public frmGame()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        Image background = Image.FromFile("zebra.jpg");
        Image bulet = Image.FromFile("explosion.png");
        Image explode = Image.FromFile("ajkula.jpg");
        Image tbird = Image.FromFile("lav.jpg");
        Image[] images = new Image[6];
        Image ship;

        int x, y;
        int targetX, targetY;
        int targetDX, targetDY;
        int buletX, buletY;
        int buletDY;
        int score;
        int count, countdown;
        bool hit;

        private void btnStart_Click(object sender, EventArgs e)
        {
            NewGame();
            timer1.Enabled = true;
            timer2.Enabled = true;
            timer3.Enabled = true;
        }

        private void NewGame()
        {
            score = 0;
            countdown = 300;
            axWindowsMediaPlayer1.URL = "cs.mp3";
            Initialize();

        }

        private void Initialize()
        {
            hit = false;
            count = 0;
            targetX = r.Next(10, 200);
            targetY = r.Next(10, 200);
            targetDX = 20;
            targetDY = 10;

            buletX = ClientSize.Width + 100;
            buletY = ClientSize.Height + 100;

            buletDY = 0;
        }

        private void frmGame_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawStuff(g);
        }



        Random r = new Random();

        SoundPlayer explosion = new SoundPlayer();

        private void timer1_Tick(object sender, EventArgs e)
        {
            buletY += buletDY;
            targetX += targetDX;
            targetY += targetDY;

            if (buletY <= 0)
            {
                buletDY = 0;
                buletX = ClientSize.Width + 100;
                buletDY = ClientSize.Height + 100;
            }

            if (targetX > ClientSize.Width - tbird.Width)
            {
                targetDX = -targetDX;
            }
            if (targetX <= 0)
            {
                targetDX = -targetDX;
            }
            if (targetY <= 0)
            {
                targetDY = -targetDY;
            }
            if (targetY > ClientSize.Height - tbird.Height)
            {
                targetDY = -targetDY;
            }

            if ((buletX + bulet.Width > targetX) && (buletX < targetX + tbird.Width) && (buletY + bulet.Height > targetY) && (buletY < targetY + tbird.Height) && (hit == false))
            {
                score += 100;
                hit = true;
            }
            Invalidate();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            ship = images[count];
            count++;
            if (count > 5)
            {
                count = 0;
            }
            Invalidate();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            countdown--;
            Invalidate();
        }

        private void frmGame_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad4)
            {
                x -= 10;
            }
            if (e.KeyCode == Keys.NumPad6)
            {
                x += 10;
            }
            if (e.KeyCode == Keys.NumPad8)
            {
                buletX = x + 35;
                buletY = y - 10;
                buletDY = -20;
                laser.Play();
            }
            Invalidate();
        }

        SoundPlayer backMusic = new SoundPlayer();
        SoundPlayer laser = new SoundPlayer();

        private void DrawStuff(Graphics g)
        {
            Font font = new Font("Arial", 9, FontStyle.Bold);
            SolidBrush yellowBrush = new SolidBrush(Color.Yellow);
            Font gameover = new Font("Arial", 40, FontStyle.Bold);
            SolidBrush blueBrush = new SolidBrush(Color.Red);

            g.DrawImage(background, 0, 0);

            g.DrawString("Move left or right with 4 and 6 key", font, yellowBrush, ClientRectangle.Width / 2 - 50, 5);
            g.DrawString("Shoot with the 8 key", font, yellowBrush, ClientRectangle.Width / 2 - 50, 20);
            g.DrawString("Current score: " + score.ToString(), font, yellowBrush, 5, 5);
            g.DrawString("Time left: " + countdown.ToString(), font, yellowBrush, 5, 20);

            g.DrawImage(ship, x, y);
            g.DrawImage(tbird, targetX, targetY);
            g.DrawImage(bulet, buletX, buletY);

            if (hit)
            {
                g.DrawImage(explode, targetX, targetY);
                explosion.Play();
                Initialize();
            }

            if (countdown <= 0)
            {
                g.DrawString("GAME OVER", gameover, blueBrush, ClientSize.Width / 2 - 170, ClientSize.Height / 2-60);
                g.DrawString("Press start button to play again", font, yellowBrush, ClientSize.Width / 2 - 60, ClientSize.Height-80);
                StopGame();
            }
        }

        private void StopGame()
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            axWindowsMediaPlayer1.close();
        }


        private void frmGame_Load(object sender, EventArgs e)
        {
            hit = false;
            score = 0;
            countdown = 300;

            targetX = 40;
            targetY = 25;

            buletX = ClientSize.Width + 100;
            buletY = ClientSize.Height + 100;

            for(int i = 0; i <= 5; i++)
            {
                images[i] = Image.FromFile("Car" + i+".png");
            }
            ship = images[0];

            x = ClientSize.Width / 3;
            y = ClientSize.Height - ship.Height;

            explosion.SoundLocation = "expl.wav";
            backMusic.SoundLocation = "backMusic.wav";
            laser.SoundLocation = "laser.wav";
            explosion.LoadAsync();
            backMusic.LoadAsync();
            laser.LoadAsync();
            this.Focus();

        }
    }
}
