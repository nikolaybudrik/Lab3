using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Progect
{
    struct DataItem
    {
        public double x { get; set; }
        public double y { get; set; }
        public Vector2 Vector { get; set; }
        public DataItem(double x, double y, Vector2 Vector)
        {
            this.x = x;
            this.y = y;
            this.Vector = Vector;
        }
        public String ToLongString(String format)
        {
            return "X:" + x.ToString(format) + " Y:" + y.ToString(format) + " <" + Vector.X.ToString(format)
                                + "  " + Vector.Y.ToString(format) + "> Lengnt:" + Vector.Length().ToString(format);
        }
        public override string ToString()
        {
            return x + " " + y + " " + Vector;
        }
    }
}
