using System;
using System.Collections.Generic;

namespace LZWArchiver
{
    static class Program
    {
        static void Main(string[] args)
        {
            new Encoder("test.txt", "out.txt").Encode();
            new Decoder("out.txt", "recovered.txt").Decode();
        }
    }
}
            
