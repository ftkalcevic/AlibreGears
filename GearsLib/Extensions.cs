using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GearsLib
{
    public static class Extensions
    {
        public static Int32 Parse(this Int32 o, string s, Int32 defaultValue)
        {
            Int32 value;
            if (Int32.TryParse(s, out value))
                return value;
            else
                return defaultValue;
        }

        public static double Parse(this double o, string s, double defaultValue)
        {
            double value;
            if (double.TryParse(s, out value))
                return value;
            else
                return defaultValue;
        }

        public static PointF MirrorY(this PointF o)
        {
            return new PointF(o.X, -o.Y);
        }
    };

}
