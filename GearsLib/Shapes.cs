using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GearsLib
{
    public class Point
    {
        public double x, y;
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Point Rotate(double rads)  // rotate pt by rads radians about origin
        {
            var sinA = Math.Sin(rads);
            var cosA = Math.Cos(rads);

            return new Point(x * cosA - y * sinA, x * sinA + y * cosA);
        }

        static public implicit operator PointF(Point p)
        {
            return new PointF((float)p.x, (float)p.y);
        }
    }


    public class Shape
    {
    }

    public class MoveTo : Shape
    {
        public double x, y;
        public MoveTo(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public class LineTo : Shape
    {
        public double x, y;
        public LineTo(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public class Bezier : Shape
    {
        public List<Point> points;
        public Bezier( params double[] coords)
        {
            points = new List<Point>();
            for ( int i = 0; i < coords.Length; i+=2)
            {
                points.Add(new Point(coords[i], coords[i + 1]));
            }
        }
    }
    public class Arc : Shape
    {
        public double rx, ry; // radii
        public double Xrotation;
        public bool largeArc;
        public bool sweep;
        public double x, y;    // end point
        public Arc(double rx, double ry, double Xrotation, bool largeArc, bool sweep, double x, double y)
        {
            this.rx = rx;
            this.ry = ry;
            this.Xrotation = Xrotation;
            this.largeArc = largeArc;
            this.sweep = sweep;
            this.x = x;
            this.y = y;
        }
    }
}
