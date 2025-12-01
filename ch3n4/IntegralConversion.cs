using System;

namespace ch3n4
{
    public class IntegralConversion
    {
        public static void Main(string[] args)
        {
            sbyte a = 127;
            Console.WriteLine(a);

            int b = (int)a;
            Console.WriteLine(b);

            int x = 128; // sbyte 타입의 최대값 127보다 큰 수
            Console.WriteLine(x);

            sbyte y = (sbyte)x;
            Console.WriteLine(y); // 오버플로 발생
        }
    }
}
