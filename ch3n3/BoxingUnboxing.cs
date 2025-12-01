using System;

namespace ch3n3
{
    public class BoxingUnboxing
    {
        public static void Main(string[] args)
        {
            int a = 123;
            object b = (object)a; // a에 담긴 값을 박싱해서 힙에 저장한다.
            int c = (int)b; // b에 담긴 값을 언박싱해서 스택에 저장한다.

            Console.WriteLine(a);
            Console.WriteLine(b);
            Console.WriteLine(c);

            double x = 3.1414213;
            object y = x; // x에 담긴 값을 박싱해서 힙에 저장한다.
            double z = (double)y; // y에 담긴 값을 언박싱해서 스택에 저장한다.

            Console.WriteLine(x);
            Console.WriteLine(y);
            Console.WriteLine(z);
        }
    }
}
