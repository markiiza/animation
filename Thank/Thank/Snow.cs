using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Thank
{
    internal class Snow
    {
        int diametr;
        int speed;
        double angle;

        public int X { get; private set; }
        public int Y { get; private set; }
        public Snow(int diametr, int speed, double angle, int x, int y)
        {
            this.diametr = diametr;
            this.speed = speed;
            this.angle = angle;

            X = x; Y = y; 
        }   

        public void Paint(Graphics g)
        {
            g.FillEllipse(Brushes.White, X-diametr/2, Y-diametr/2,diametr,diametr);
        }

    }
}
