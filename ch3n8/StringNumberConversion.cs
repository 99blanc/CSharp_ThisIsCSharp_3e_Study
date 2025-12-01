using System;

namespace ch3n8
{
    public class StringNumberConversion
    {
        public static void Main(string[] args)
        {
            int a = 123;
            string b = a.ToString();
            Console.WriteLine(b);

            float c = 3.14f;
            string d = c.ToString();
            Console.WriteLine(d);

            string e = "1.2345";
            float h = float.Parse(g);
            Console.WriteLine(h);
        }
    }
}
