using System;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using System.Linq;

namespace Progect
{
    delegate Vector2 FdblVector2(double x, double y);

    static class VectorMetods
    {
        public static Vector2 foo(double x, double y)
        {
            return new Vector2((float) (x * x * x), (float)(y));
        }
        public static Vector2 foo2(double x, double y)
        {
            return new Vector2((float)x, (float)(y * y));
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            V3DataArray first = new V3DataArray("Str", DateTime.UtcNow, 5, 2, 0.4, 1, VectorMetods.foo);
            float[] left = { 0F, 0.5F, 0F };
            float[] right = { 0.5F, 2F, 2F };

            Console.WriteLine(first.ToLongString("F3"));

            first.Integrals(left, right);

            Console.WriteLine();
            for (int j = 0; j < 3; j++)
            {
                Console.WriteLine(first.Value[j][0]);
            }
            Console.WriteLine("Следующий y");
            for (int j = 0; j < 3; j++)
            {
                Console.WriteLine(first.Value[j][1]);
            }
            Console.WriteLine("Для обоих y, сумма первых двух интегралов равно 3-ему");


        }
    }
}
