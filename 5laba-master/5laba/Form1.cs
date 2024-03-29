﻿using _5laba.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace _5laba
{
    public partial class Form1 : Form
    {
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        Circle circle1, circle2;
        EvilCircle evilCircle;
        int counter = 0;
        public static Random rand = new Random();
        public Form1()
        {
            InitializeComponent();
            label1.Text = "Счётчик очков: " + counter;
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
            };

            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            player.OnCircleOverlap += (c) =>
            {
                objects.Remove(c);
                circle1 = new Circle(rand.Next(pbMain.Width), rand.Next(pbMain.Height), 0);
                objects.Add(circle1);
                counter++;
            };

            player.OnEvilCircleOverlap += (d) =>
            {
                objects.Remove(d);
                evilCircle = new EvilCircle(rand.Next() % pbMain.Width, rand.Next() % pbMain.Height, 0);
                objects.Add(evilCircle);
                counter-=2;
            };
            circle1 = new Circle(rand.Next(pbMain.Width), rand.Next(pbMain.Height), 0);
            circle2 = new Circle(rand.Next(pbMain.Width), rand.Next(pbMain.Height), 0);

            evilCircle = new EvilCircle(rand.Next() % pbMain.Width, rand.Next() % pbMain.Height, 0);

            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);

      
            objects.Add(evilCircle);
            objects.Add(circle1);
            objects.Add(circle2);
            objects.Add(marker);
            objects.Add(player);
        }

        private void pdMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            updatePlayer();


            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g)) 
                {
                    player.Overlap(obj);
                }
            }
            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
            label1.Text = "Счётчик очков: " + counter;
        }

        private void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;

                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            player.X += player.vX;
            player.Y += player.vY;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pbMain.Invalidate();
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            if(marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker);
            }
            marker.X = e.X;
            marker.Y = e.Y;
        }
       
    }
}
