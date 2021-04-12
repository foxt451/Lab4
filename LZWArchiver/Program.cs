using System;

namespace LZWArchiver
{
    static class Program
    {
        static void Main(string[] args)
        {
            new Encoder("test.bin", "out.bin").Encode();
        }
    }
}
            
