using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progect
{
    abstract class V3Data : IEnumerable<DataItem>
    {
        public String Str { get; protected set; }
        public DateTime Dt { get; protected set; }
        protected int count;
        protected double maxDistance;
        public abstract int Count { get; }
        public abstract double MaxDistance { get; }
        public V3Data(String str, DateTime dt)
        {
            Str = str;
            Dt = dt;
        }

        abstract public String ToLongString(String format);

        public override String ToString()
        {
            return (Str + " Date:" + Dt.ToString() + " Count:" +
                Count.ToString() + " MaxDistance:" + MaxDistance.ToString());
        }

        public String ToString(String format)
        {
            return (Str + " Date:" + Dt.ToString() + " Count:" +
                Count.ToString() + " MaxDistance:" + MaxDistance.ToString(format));
        }

        public abstract IEnumerator<DataItem> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
