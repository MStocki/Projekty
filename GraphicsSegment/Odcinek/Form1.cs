using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Odcinek
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            label1.BringToFront();
        }

        Point start = new Point();
        Point end = new Point();
        private void AlgorytmPrzyrostowy(Point start,Point end)
        {
            double dy, dx, y, m,x;
            dy = end.Y - start.Y;
            dx = end.X - start.X;
            m = dy / dx;
           
            if (Math.Abs(m)<1)
            {
                if (start.X >= end.X)
                {
                    Point t = start;
                    start = end;
                    end = t;
                }
                y = start.Y;
                for (x = start.X; x <= end.X; x++)
                {
                    ((Bitmap)pictureBox1.Image).SetPixel((int)x, (int)Math.Floor(y + 0.5), Color.FromArgb(0, 0, 0));
                    y += m;
                }
            }
            else
            {
                double m2;
                if (start.Y >= end.Y)
                {
                 Point t = start;
                 start = end;
                 end = t;
                }
                if (end.X - start.X == 0) m2 = 0;
                else m2 = 1 / m;
                x = start.X;
                for (y = start.Y; y <= end.Y; y++)
                {
                    ((Bitmap)pictureBox1.Image).SetPixel((int)Math.Floor(x + 0.5), (int)y, Color.FromArgb(0, 0, 0));
                    x += m2;
                }
            }
          
            pictureBox1.Refresh();
        }
        private void AlgorytmPktSrodkowy(Point start, Point end)
        {
           
            int d, dx, dy, E, NE, xi, yi, x = start.X, y = start.Y;
            if (start.X < end.X)
            {
                xi = 1;
                dx = end.X - start.X;
            }
            else
            {
                xi = -1;
                dx = start.X - end.X;
            }
            if (start.Y < end.Y)
            {
                yi = 1;
                dy = end.Y - start.Y;
            }
            else
            {
                yi = -1;
                dy = start.Y - end.Y;
            }
            if (dx > dy)
            {
                E = (dy - dx) * 2;
                NE = dy * 2;
                d = NE - dx;
               
                while (x != end.X)
                {
                   
                    if (d < 0)
                    {
                        d += NE;
                        x += xi;
                      
                    }
                    else
                    {
                        x += xi;
                        y += yi;
                        d += E;
                    }
                    ((Bitmap)pictureBox1.Image).SetPixel(x, y, Color.FromArgb(0, 0, 0));
                }
            }
          
            else
            {
                E = (dx - dy) * 2;
                NE = dx * 2;
                d = NE - dy;
              
                while (y != end.Y)
                {
                   
                    if (d < 0)
                    {
                       
                        d += NE;
                        y += yi;
                    }
                    else
                    {
                         x += xi;
                        y += yi;
                        d += E;
                    }
                    ((Bitmap)pictureBox1.Image).SetPixel(x, y, Color.FromArgb(0, 0, 0));
                }
            }
           
            pictureBox1.Refresh();
        }
        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
       
            if (e.Button==MouseButtons.Left)
            {
                if(start.IsEmpty)
                {
                    start = this.PointToClient(MousePosition);
                    for (int i = -1; i <2 ; i++)
                    {
                        ((Bitmap)pictureBox1.Image).SetPixel(start.X+i, start.Y+i, Color.FromArgb(0, 0, 0));
                        ((Bitmap)pictureBox1.Image).SetPixel(start.X - i, start.Y + i, Color.FromArgb(0, 0, 0));
                    }
                    pictureBox1.Refresh();
                    label1.Text = "Zaznacz koniec";
                    textBox1.Text = "X=" + start.X.ToString() + " Y=" + start.Y.ToString();
                   
                }
                else
                {
                    end = this.PointToClient(MousePosition);
                    for (int i = -1; i < 2; i++)
                    {
                        ((Bitmap)pictureBox1.Image).SetPixel(end.X + i, end.Y + i, Color.FromArgb(0, 0, 0));
                        ((Bitmap)pictureBox1.Image).SetPixel(end.X - i, end.Y + i, Color.FromArgb(0, 0, 0));
                    }
                    pictureBox1.Refresh();
                    textBox2.Text = "X=" + end.X.ToString() + " Y=" + end.Y.ToString();
                    //wywołanie funkcji
                    if (algorymt1.Checked) AlgorytmPrzyrostowy(start, end);
                    else AlgorytmPktSrodkowy(start, end);
                    start = new Point();
                    end = new Point();
                    label1.Text = "Zaznacz początek";
                }
            }
        }
       
        private int LabelX, LabelY;
        
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            LabelX = e.Location.X;
            LabelY = e.Location.Y;
            label1.Location = new Point(LabelX-20,LabelY+20);
        }
        private void pictureBox1_MouseHover(object sender,EventArgs e)
        {
            label1.Visible = true;
        }

        private void reset_Click(object sender, EventArgs e)
        {
            pictureBox1.Image= new Bitmap(pictureBox1.Width, pictureBox1.Height);
            textBox1.Text = "";
            textBox2.Text = "";

        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            label1.Visible = false;
        }
    }
}
