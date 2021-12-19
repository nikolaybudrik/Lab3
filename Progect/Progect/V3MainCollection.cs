using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progect
{
    class V3MainCollection
    {

        private List<V3Data> V3List;
        private int count;
        public int Count
        {
            get { return count; }
        }

        public double Average
        {
            get
            {
                if (Count == 0)
                    return double.NaN;
                IEnumerable<double> tmp1 = from item in V3List select item.Sum(x => Math.Sqrt(x.x * x.x + x.y * x.y));
                double sum = tmp1.Sum();
                IEnumerable<int> tmp2 = from item in V3List select item.Count;
                int count = tmp2.Sum();
                return sum / count;
            }
        }

        public IEnumerable <float> Diferens {
            get
            {
                if (Count == 0)
                    return null;
                return from item in V3List select item.Count != 0 ? item.Max(x => x.Vector.Length()) - item.Min(x => x.Vector.Length()) : 0;
            }
        }

        public IEnumerable<IGrouping<double, DataItem>> Group_X
        {
            get
            {
                if (Count == 0)
                    return null;
                IEnumerable<DataItem> AllData = V3List.Aggregate<IEnumerable<DataItem>>((tmp, x) => tmp.Concat(x));
                return from item in AllData group item by item.x;
            }
        }

        public V3Data this[int i]
        {
            get { return V3List[i]; }
        }

        public V3MainCollection()
        {
            count = 0;
            V3List = new List<V3Data>();
        }

        public bool Contains(String ID)
        {
            return V3List.Exists(D => D.Str == ID);
        }

        public bool Add(V3Data v3Data)
        {
            if (!Contains(v3Data.Str))
            {
                V3List.Add(v3Data);
                count++;
                return true;
            }
            return false;
        }

        public string ToLongString(String format)
        {
            String ret = "";
            for (int i = 0; i < count - 1; i++)
            {
                ret += V3List[i].ToLongString(format) + "\n\n";
            }
            ret += V3List[count - 1].ToLongString(format);
            return ret;
        }

        public override string ToString()
        {
            String ret = "";
            for (int i = 0; i < count - 1; i++)
            {
                ret += V3List[i].ToString() + "\n";
            }
            ret += V3List[count - 1].ToString();
            return ret;
        }
    }
}
