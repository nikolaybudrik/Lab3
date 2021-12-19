using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Progect
{
    class V3DataArray : V3Data, IEnumerable<DataItem>
    {
        public int CountX { get; private set; }
        public int CountY { get; private set; }
        public double StepX { get; private set; }
        public double StepY { get; private set; }
        public Vector2[,] Array { get; private set; }

        public float[] LeftBorder{ get; set; }
        public float[] RightBorder { get; set; }
        public Vector2[][] Value { get; private set; }


        public V3DataArray(string str, DateTime dt) : base(str, dt)
        {
            Array = new Vector2[0, 0];
        }
        public V3DataArray(string str, DateTime dt, int CountX, int CountY,
                           Double StepX, Double StepY, FdblVector2 F) : base(str, dt)
        {
            this.CountX = CountX;
            this.CountY = CountY;
            this.StepX = StepX;
            this.StepY = StepY;
            Array = new Vector2[CountX, CountY];
            for (int i = 0; i < CountX; i++)
            {
                for (int j = 0; j < CountY; j++)
                {
                    Array[i, j] = F(i * StepX, j * StepY);
                }
            }
            count = CountX * CountY;
            if (CountX == 0 || CountY == 0)
                maxDistance = 0.0;
            else
                maxDistance = Math.Sqrt((CountX - 1) * (CountX - 1) * StepX * StepX +
                                    (CountY - 1) * (CountY - 1) * StepY * StepY);
        }

        public override int Count { get { return count; } }

        public override double MaxDistance { get { return maxDistance; } }

        public override string ToString()
        {
            return (GetType().ToString() + ' ' + base.ToString() + " CountX:" + CountX.ToString() + " CountY:" +
                CountY.ToString() + " StepX:" + StepX.ToString() + " StepY:" + StepY.ToString());
        }

        public override string ToLongString(string format)
        {
            String ret = (GetType().ToString() + ' ' + ToString(format) + " CountX:" + CountX.ToString() + " CountY:" +
                CountY.ToString() + " StepX:" + StepX.ToString(format) + " StepY:" + StepY.ToString(format));
            ret += "\nDots Item:";
            for (int i = 0; i < CountX; i++)
            {
                for (int j = 0; j < CountY; j++)
                {
                    ret += ($"\nX:" + (i * StepX).ToString(format) + " Y:" + (j * StepY).ToString(format) +
                        " " + Array[i, j].ToString(format) + " Lenght:" + Array[i, j].Length().ToString(format));
                }
            }
            return ret;
        }

        public override IEnumerator<DataItem> GetEnumerator()
        {
            for (int i = 0; i < CountX; i++)
                for (int j = 0; j < CountY; j++)
                {
                    yield return new DataItem(i * StepX, j * StepY, Array[i, j]);
                }
        }

        public static explicit operator V3DataList(V3DataArray DataArray)
        {
            V3DataList DataList = new V3DataList(DataArray.Str, DataArray.Dt);
            for (int i = 0; i < DataArray.CountX; i++)
            {
                for (int j = 0; j < DataArray.CountY; j++)
                {
                    DataList.Add(new DataItem(i * DataArray.StepX, j * DataArray.StepY, DataArray.Array[i, j]));
                }
            }
            return DataList;
        }

        public bool SaveBinary(string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.OpenOrCreate);
                BinaryWriter writer = new BinaryWriter(fs);
                writer.Write(Str);
                writer.Write(Dt.ToString());
                writer.Write(CountX);
                writer.Write(CountY);
                writer.Write(StepX);
                writer.Write(StepY);
                for (int i = 0; i < CountX; i++)
                {
                    for (int j = 0; j < CountY; j++)
                    {
                        writer.Write(Array[i, j].X);
                        writer.Write(Array[i, j].Y);
                    }
                }
                writer.Write(maxDistance);
                writer.Write(count);
                writer.Close();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        static public bool LoadBinary(string filename, ref V3DataArray v3)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open);
                BinaryReader reader = new BinaryReader(fs);
                String str = reader.ReadString();
                DateTime dt = Convert.ToDateTime(reader.ReadString());
                if (v3 == null)
                    v3 = new V3DataArray(str, dt);
                else
                {
                    v3.Str = str;
                    v3.Dt = dt;
                }
                v3.CountX = reader.ReadInt32();
                v3.CountY = reader.ReadInt32();
                v3.StepX = reader.ReadDouble();
                v3.StepY = reader.ReadDouble();
                v3.Array = new Vector2[v3.CountX, v3.CountY];
                for (int i = 0; i < v3.CountX; i++)
                {
                    for (int j = 0; j < v3.CountY; j++)
                    {
                        float tmpX = reader.ReadSingle();
                        float tmpY = reader.ReadSingle();
                        v3.Array[i, j] = new Vector2(tmpX, tmpY);
                    }
                }
                v3.maxDistance = reader.ReadDouble();
                v3.count = reader.ReadInt32();
                reader.Close();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        [DllImport("..\\..\\..\\..\\x64\\Debug\\CPP_Dll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Intagrate(float[] Array, int nx, int ny, double stepX, int count, float[] left, float[] right, float[] res);

        public bool Integrals(float[] left, float[] right)
        {
            int N = left.Length;
            if (right.Length != N)
                return false;

            LeftBorder = new float[N];
            RightBorder = new float[N];

            for (int i = 0; i < N; i++)
            {
                LeftBorder[i] = left[i];
                RightBorder[i] = right[i];
            }


            float[] answer = new float[CountY * N * 2];
            float[] Array_R = new float[CountX * CountY * 2];
            for(int i = 0; i < CountX; i++)
            {
                for(int j = 0; j < CountY; j++)
                {
                    Array_R[(i * CountY + j) * 2] = Array[i, j].X;
                    Array_R[(i * CountY + j) * 2 + 1] = Array[i, j].Y;
                }
            }
            int status = Intagrate(Array_R, CountX, CountY, StepX, N, left, right, answer);
            Value = new Vector2[N][];
            for (int i = 0; i < N; i ++)
            {
                Value[i] = new Vector2[CountY];
            }
            if (status == 0)
            {
                for(int i = 0; i < CountY * N * 2; i += 2)
                {
                    Value[i / (CountY * 2)][(i / 2) % CountY] = new Vector2(answer[i], answer[i + 1]);
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
